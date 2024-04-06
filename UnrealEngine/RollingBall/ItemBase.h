#pragma once

#include "GameFramework/RotatingMovementComponent.h"
#include "Components/SphereComponent.h"
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "ItemBase.generated.h"

UCLASS()
class ROLLINGBALL_API AItemBase : public AActor
{
	GENERATED_BODY()

private:
	UPROPERTY(EditAnywhere)
	TObjectPtr<UStaticMeshComponent> StaticMesh;

private:
	UPROPERTY(EditAnywhere)
	TObjectPtr<USphereComponent> Collision;

private:
	UPROPERTY(EditAnywhere)
	TObjectPtr<URotatingMovementComponent> RotatingMovementComponent;

public:
	AItemBase();

protected:
	UFUNCTION(BlueprintCallable, meta = (KeyWords = "on collision begin overlap"))
	void OnCollisionBeginOverlap(AActor* OtherActor);

protected:
	virtual void OnTouchedByPlayerBall();
};