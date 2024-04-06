#pragma once

#include "Components/BoxComponent.h"
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "KillVolume.generated.h"

UCLASS()
class ROLLINGBALL_API AKillVolume : public AActor
{
	GENERATED_BODY()

private:
	TObjectPtr<UBoxComponent> Collision;

protected:
	UFUNCTION(BlueprintCallable, meta = (KeyWords = "set up kill volume"))
	void SetupKillVolume(UBoxComponent* BoxComponent);

protected:
	UFUNCTION(BlueprintCallable, meta = (KeyWords = "on collision begin overlap"))
	void OnCollisionBeginOverlap(AActor* OtherActor);
};