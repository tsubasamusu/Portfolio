#include "TrapFloor.h"
#include "PlayerBall.h"

void ATrapFloor::SetupTrapFloor(UBoxComponent* BoxComponent)
{
	if (BoxComponent != nullptr) BoxComponent->SetCollisionProfileName("OverlapAllDynamic");
}

void ATrapFloor::OnCollisionBeginOverlap(AActor* OtherActor)
{
	APlayerBall* PlayerBall = Cast<APlayerBall>(OtherActor);

	if (PlayerBall != nullptr)PlayerBall->DamageToPlayer();
}