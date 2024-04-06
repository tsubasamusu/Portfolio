#pragma once

#include "Components/BoxComponent.h"
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "GoalPoint.generated.h"

UCLASS()
class ROLLINGBALL_API AGoalPoint : public AActor
{
	GENERATED_BODY()

private:
	UPROPERTY(EditAnywhere)
	FName NextLevelName = NAME_None;

private:
	TObjectPtr<UBoxComponent> Collision;

protected:
	UFUNCTION(BlueprintCallable, meta = (KeyWords = "set up goal point"))
	void SetupGoalPoint(UBoxComponent* BoxComponent);

protected:
	UFUNCTION(BlueprintCallable, meta = (KeyWords = "on collision begin overlap"))
	void OnCollisionBeginOverlap(AActor* OtherActor);
};