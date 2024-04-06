#include "KillVolume.h"
#include "PlayerBall.h"
#include "GameMode_Main.h"
#include "Kismet/GameplayStatics.h"

void AKillVolume::SetupKillVolume(UBoxComponent* BoxComponent)
{
	Collision = BoxComponent;

	if (Collision != nullptr) Collision->SetCollisionProfileName("OverlapAllDynamic");
}

void AKillVolume::OnCollisionBeginOverlap(AActor* OtherActor)
{
	APlayerBall* PlayerBall = Cast<APlayerBall>(OtherActor);

	if (PlayerBall == nullptr) return;

	AGameMode_Main* GameMode = Cast<AGameMode_Main>(UGameplayStatics::GetGameMode(GetWorld()));

	if (GameMode == nullptr) return;

	GameMode->RespawnPlayer(PlayerBall);
}