#pragma once

#include "Components/CapsuleComponent.h"
#include "Components/SceneComponent.h"
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "Checkpoint.generated.h"

UCLASS()
class ROLLINGBALL_API ACheckpoint : public AActor
{
	GENERATED_BODY()

private:
	FVector RespawnLocation;

protected:
	UFUNCTION(BlueprintCallable, meta = (KeyWords = "set up check point"))
	void SetupCheckpoint(USceneComponent* RespawnTransformComponent, UCapsuleComponent* CapsuleComponent);

protected:
	UFUNCTION(BlueprintCallable, meta = (KeyWords = "on collision begin overlap"))
	void OnCollisionBeginOverlap(AActor* OtherActor);
};