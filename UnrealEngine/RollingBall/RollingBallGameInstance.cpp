#include "RollingBallGameInstance.h"

void URollingBallGameInstance::ResetAllCounts()
{
	const URollingBallGameInstance* DefaultRollingBallGameInstance = GetDefault<URollingBallGameInstance>();

	if (DefaultRollingBallGameInstance == nullptr) return;

	this->GotCoinsCount = DefaultRollingBallGameInstance->GotCoinsCount;

	this->PlayerHealthPoint = DefaultRollingBallGameInstance->PlayerHealthPoint;
}

void URollingBallGameInstance::AddGotCoinsCount()
{
	GotCoinsCount++;
}

void URollingBallGameInstance::DecreasePlayerHealthPoint()
{
	PlayerHealthPoint--;
}

void URollingBallGameInstance::AddPlayerHealthPoint()
{
	PlayerHealthPoint++;
}

int URollingBallGameInstance::GetCurrentPlayerHealthPoint()
{
	return PlayerHealthPoint;
}

int URollingBallGameInstance::GetGotCoinsCount()
{
	return GotCoinsCount;
}