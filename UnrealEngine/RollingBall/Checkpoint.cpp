#include "Checkpoint.h"
#include "PlayerBall.h"
#include "Kismet/GameplayStatics.h"
#include "GameMode_Main.h"

void ACheckpoint::SetupCheckpoint(USceneComponent* RespawnTransformComponent, UCapsuleComponent* CapsuleComponent)
{
	if (RespawnTransformComponent != nullptr) RespawnLocation = RespawnTransformComponent->GetComponentToWorld().GetLocation();

	if (CapsuleComponent != nullptr) CapsuleComponent->SetCollisionProfileName("OverlapAllDynamic");
}

void ACheckpoint::OnCollisionBeginOverlap(AActor* OtherActor)
{
	if (Cast<APlayerBall>(OtherActor) == nullptr) return;

	AGameMode_Main* GameMode = Cast<AGameMode_Main>(UGameplayStatics::GetGameMode(GetWorld()));

	if (GameMode == nullptr) return;

	if (!GameMode->RespawnLocation.Equals(RespawnLocation)) GameMode->RespawnLocation = RespawnLocation;
}