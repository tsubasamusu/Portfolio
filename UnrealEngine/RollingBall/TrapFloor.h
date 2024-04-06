#pragma once

#include "Components/BoxComponent.h"
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "TrapFloor.generated.h"

UCLASS()
class ROLLINGBALL_API ATrapFloor : public AActor
{
	GENERATED_BODY()
	
protected:
	UFUNCTION(BlueprintCallable, meta = (KeyWords = "set up trap floor"))
	void SetupTrapFloor(UBoxComponent* BoxComponent);

protected:
	UFUNCTION(BlueprintCallable, meta = (KeyWords = "on collision begin overlap"))
	void OnCollisionBeginOverlap(AActor* OtherActor);
};