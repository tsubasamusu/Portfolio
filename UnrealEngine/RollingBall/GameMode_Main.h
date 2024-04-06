#pragma once

#include "PlayerBall.h"
#include "CoreMinimal.h"
#include "GameFramework/GameModeBase.h"
#include "GameMode_Main.generated.h"

UCLASS()
class ROLLINGBALL_API AGameMode_Main : public AGameModeBase
{
	GENERATED_BODY()

public:
	FVector RespawnLocation;

protected:
	UFUNCTION(BlueprintCallable, meta = (KeyWords = "set up game mode"))
	void SetupGameMode();

public:
	void RespawnPlayer(APlayerBall* PlayerBall);

public:
	void RestartGame();
};