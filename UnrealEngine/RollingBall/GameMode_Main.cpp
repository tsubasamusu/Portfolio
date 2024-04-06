#include "GameMode_Main.h"
#include "Kismet/GameplayStatics.h"
#include "GameFramework/PlayerStart.h"
#include "PlayerBall.h"
#include "Kismet/KismetSystemLibrary.h"
#include "RollingBallGameInstance.h"

void AGameMode_Main::SetupGameMode()
{
	const APlayerStart* PlayerStart = Cast<APlayerStart>(UGameplayStatics::GetActorOfClass(GetWorld(), APlayerStart::StaticClass()));

	if (PlayerStart != nullptr) RespawnLocation = PlayerStart->GetActorLocation();
}

void AGameMode_Main::RespawnPlayer(APlayerBall* PlayerBall)
{
	URollingBallGameInstance* GameInstance = Cast<URollingBallGameInstance>(UGameplayStatics::GetGameInstance(GetWorld()));

	GameInstance->DecreasePlayerHealthPoint();

	if (GameInstance->GetCurrentPlayerHealthPoint() <= 0)
	{
		RestartGame();

		return;
	}

	FActorSpawnParameters ActorSpawnParameters;

	ActorSpawnParameters.Instigator = UGameplayStatics::GetPlayerPawn(GetWorld(), 0);

	const APlayerStart* PlayerStart = Cast<APlayerStart>(UGameplayStatics::GetActorOfClass(GetWorld(), APlayerStart::StaticClass()));

	if (PlayerStart == nullptr) return;

	PlayerBall->SetActorLocation(RespawnLocation);

	PlayerBall->ClearPhysics();
}

void AGameMode_Main::RestartGame()
{
	URollingBallGameInstance* GameInstance = Cast<URollingBallGameInstance>(UGameplayStatics::GetGameInstance(GetWorld()));

	if (GameInstance != nullptr)GameInstance->ResetAllCounts();

	const FString CurrentLevelName = UGameplayStatics::GetCurrentLevelName(GetWorld());

	UGameplayStatics::OpenLevel(GetWorld(), FName(*CurrentLevelName));
}