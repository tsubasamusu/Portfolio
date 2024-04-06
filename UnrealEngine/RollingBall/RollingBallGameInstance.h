#pragma once

#include "CoreMinimal.h"
#include "Engine/GameInstance.h"
#include "RollingBallGameInstance.generated.h"

UCLASS()
class ROLLINGBALL_API URollingBallGameInstance : public UGameInstance
{
	GENERATED_BODY()

private:
	int GotCoinsCount;

private:
	int PlayerHealthPoint = 3;

public:
	void ResetAllCounts();

public:
	void AddGotCoinsCount();

public:
	void DecreasePlayerHealthPoint();

public:
	void AddPlayerHealthPoint();

public:
	int GetCurrentPlayerHealthPoint();

public:
	int GetGotCoinsCount();
};