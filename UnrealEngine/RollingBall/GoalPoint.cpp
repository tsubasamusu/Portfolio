#include "GoalPoint.h"
#include "Kismet/KismetSystemLibrary.h"
#include "PlayerBall.h"
#include "Kismet/GameplayStatics.h"

void AGoalPoint::SetupGoalPoint(UBoxComponent* BoxComponent)
{
	Collision = BoxComponent;

	if (Collision != nullptr) Collision->SetCollisionProfileName("OverlapAllDynamic");
}

void AGoalPoint::OnCollisionBeginOverlap(AActor* OtherActor)
{
	if (Cast<APlayerBall>(OtherActor) == nullptr) return;

	if (NextLevelName != NAME_None) UGameplayStatics::OpenLevel(this, NextLevelName);
}