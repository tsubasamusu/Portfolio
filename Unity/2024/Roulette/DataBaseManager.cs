using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TSUBASAMUSU.Spreadsheet;
using UnityEngine;

namespace Roulette
{
    public static class DataBaseManager
    {
        public static async UniTask<bool> TryUpdateLoginedDataAsync()
        {
            List<List<string>> cellValues;

            cellValues = (await GetDatasFromDataBaseAsync()).cellValues;

            if (cellValues == null)
            {
                ErrorDisplayerController.Instance.DisplayError(ConstData.ERROR_FAILD_TO_GET_SAVE_DATA);

                return false;
            }

            for (int i = 0; i < cellValues.Count; i++)
            {
                if (cellValues[i][ConstData.SHEET_COLUMN_SAVE_DATA_NAME - 1] != GameData.Instance.LoginedData.saveDataName) continue;

                List<Member> members;

                try
                {
                    members = JsonUtility.FromJson<SaveData>(cellValues[i][ConstData.SHEET_COLUMN_JASON_DATA - 1]).members;
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);

                    ErrorDisplayerController.Instance.DisplayError(ConstData.ERROR_CORRUPTION_SAVE_DATA);

                    return false;
                }

                GameData.Instance.LoginedData = new()
                {
                    saveDataName = GameData.Instance.LoginedData.saveDataName,
                    passcode = GameData.Instance.LoginedData.passcode,
                    members = members
                };

                return true;
            }

            return false;
        }

        public static async UniTask<SaveToDataBaseResult> TryOverwriteSaveDataAsync(SaveData saveData)
        {
            List<List<string>> cellValues = (await GetDatasFromDataBaseAsync()).cellValues;

            if (cellValues == null)
            {
                ErrorDisplayerController.Instance.DisplayError(ConstData.ERROR_FAILD_TO_GET_SAVE_DATA);

                return SaveToDataBaseResult.Error;
            }

            for (int i = 0; i < cellValues.Count; i++)
            {
                if (cellValues[i][ConstData.SHEET_COLUMN_SAVE_DATA_NAME - 1] != saveData.saveDataName) continue;

                Response_Set response = await SpreadsheetManager.SetCellValueAsync(await GameData.Instance.GetGoogleCloudJwtAsync(), SecretConstData.SPREADSHEET_ID, SecretConstData.SPREADSHEET_NAME, i + ConstData.SHEET_ROW_FIRST, ConstData.SHEET_COLUMN_JASON_DATA, JsonUtility.ToJson(saveData));

                if (response == null)
                {
                    ErrorDisplayerController.Instance.DisplayError(ConstData.ERROR_FAILED_TO_SAVE_TO_DATA_BASE);

                    return SaveToDataBaseResult.Error;
                }

                return SaveToDataBaseResult.Success;
            }

            return SaveToDataBaseResult.FaildToFindSaveData;
        }

        public static async UniTask<MakeSaveDataResult> TryMakeSaveDataAsync(SaveData saveData)
        {
            (List<List<string>> cellValues, int lastRow) data = await GetDatasFromDataBaseAsync();

            if (data.cellValues == null)
            {
                ErrorDisplayerController.Instance.DisplayError(ConstData.ERROR_FAILD_TO_GET_SAVE_DATA);

                return MakeSaveDataResult.Error;
            }

            foreach (List<string> cellValuesPerRow in data.cellValues)
            {
                if (cellValuesPerRow[ConstData.SHEET_COLUMN_SAVE_DATA_NAME - 1] == saveData.saveDataName) return MakeSaveDataResult.SameSaveDataNameAlreadyExists;
            }

            List<(int column, string value)> setValues = new()
            {
                (ConstData.SHEET_COLUMN_SAVE_DATA_NAME, saveData.saveDataName),
                (ConstData.SHEET_COLUMN_PASSCODE, saveData.passcode),
                (ConstData.SHEET_COLUMN_JASON_DATA, JsonUtility.ToJson(saveData))
            };

            foreach ((int column, string value) setValue in setValues)
            {
                Response_Set response = await SpreadsheetManager.SetCellValueAsync(await GameData.Instance.GetGoogleCloudJwtAsync(), SecretConstData.SPREADSHEET_ID, SecretConstData.SPREADSHEET_NAME, data.lastRow + 1, setValue.column, setValue.value);

                if (response == null)
                {
                    ErrorDisplayerController.Instance.DisplayError(ConstData.ERROR_FAILED_TO_SAVE_TO_DATA_BASE);

                    return MakeSaveDataResult.Error;
                }
            }

            return MakeSaveDataResult.Success;
        }

        public static async UniTask<LoginResult> TryLoginAsync(string saveDataName, string passcode)
        {
            List<List<string>> cellValues = (await GetDatasFromDataBaseAsync()).cellValues;

            if (cellValues == null)
            {
                ErrorDisplayerController.Instance.DisplayError(ConstData.ERROR_FAILD_TO_GET_SAVE_DATA);

                return LoginResult.Error;
            }

            for (int row = 0; row < cellValues.Count; row++)
            {
                if (cellValues[row][ConstData.SHEET_COLUMN_SAVE_DATA_NAME - 1] != saveDataName) continue;

                if (cellValues[row][ConstData.SHEET_COLUMN_PASSCODE - 1] != passcode) return LoginResult.EnteredUncrrectPasscode;

                string jsonData = cellValues[row][ConstData.SHEET_COLUMN_JASON_DATA - 1];

                try
                {
                    GameData.Instance.LoginedData = JsonUtility.FromJson<SaveData>(jsonData);
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);

                    ErrorDisplayerController.Instance.DisplayError(ConstData.ERROR_CORRUPTION_SAVE_DATA);

                    return LoginResult.Error;
                }

                if (GameData.Instance.saveLoginInformation) GameData.Instance.StoreLoginedData();

                return LoginResult.Success;
            }

            return LoginResult.EnteredNonExistentSaveDataName;
        }

        public static string ConvertAvailableText(string text)
        {
            foreach (string unavailableText in GameData.Instance.UnavailableTexts) text = text.Replace(unavailableText, string.Empty);

            return text;
        }

        public static async UniTask<(List<List<string>> cellValues, int lastRow)> GetDatasFromDataBaseAsync()
        {
            int lastRow = await SpreadsheetManager.GetLastRowAsync(await GameData.Instance.GetGoogleCloudJwtAsync(), SecretConstData.SPREADSHEET_ID, SecretConstData.SPREADSHEET_NAME, ConstData.SHEET_COLUMN_SAVE_DATA_NAME);

            List<List<string>> cellValues = await SpreadsheetManager.GetCellValuesAsync(await GameData.Instance.GetGoogleCloudJwtAsync(), SecretConstData.SPREADSHEET_ID, SecretConstData.SPREADSHEET_NAME, (ConstData.SHEET_ROW_FIRST, ConstData.SHEET_COLUMN_SAVE_DATA_NAME), (lastRow, ConstData.SHEET_COLUMN_JASON_DATA));

            return (cellValues, lastRow);
        }
    }
}