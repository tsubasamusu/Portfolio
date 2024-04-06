#pragma once

#include "InputMappingContext.h"
#include "InputActionValue.h"
#include "CoreMinimal.h"
#include "GameFramework/Pawn.h"
#include "PlayerBall.generated.h"

UCLASS()
class ROLLINGBALL_API APlayerBall : public APawn
{
	GENERATED_BODY()

private:
	UPROPERTY(EditAnywhere)
	float MovingSpeed = 300.0f;

private:
	UPROPERTY(EditAnywhere)
	float AdditionalSpeed = 500000000.0f;

private:
	UPROPERTY(EditAnywhere, meta = (ClampMin = "0.0", ClampMax = "90.0"))
	float MaxCameraAngle = 30.0f;

private:
	UPROPERTY(EditAnywhere)
	float JumpImpluse = 1000.0f;

private:
	UPROPERTY(EditAnywhere)
	float ReboundPower = 600.0f;

private:
	bool bCanJump;

private:
	UPROPERTY(EditAnywhere)
	bool bEnableMotionBlurForCamera;

private:
	UPROPERTY(EditAnywhere)
	TObjectPtr<UInputMappingContext> DefaultInputMappingContext;

private:
	UPROPERTY(EditAnywhere)
	TObjectPtr<UInputAction> MovementInputAction;

private:
	UPROPERTY(EditAnywhere)
	TObjectPtr<UInputAction> CameraInputAction;

private:
	UPROPERTY(EditAnywhere)
	TObjectPtr<UInputAction> JumpInputAction;

private:
	UPROPERTY(EditAnywhere)
	TObjectPtr<UInputAction> BoostInputAction;

private:
	TObjectPtr<UStaticMeshComponent> SphereStaticMesh;

private:
	TObjectPtr<UCameraComponent> Camera;

private:
	TObjectPtr<USpringArmComponent> SpringArm;

protected:
	UFUNCTION(BlueprintCallable, meta = (KeyWords = "setup set up player ball"))
	void SetupPlayerBall(UStaticMeshComponent* StaticMeshComponent, USpringArmComponent* SpringArmComponent, UCameraComponent* CameraComponent);

private:
	void SetupComponentSettings();

private:
	void SetupEnhancedInputMappingContext();

public:
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

private:
	void ControlBallMovement(const FInputActionValue& InputActionValue);

private:
	void ControlLooking(const FInputActionValue& InputActionValue);

private:
	void Jump(const FInputActionValue& InputActionValue);

protected:
	virtual void NotifyHit(class UPrimitiveComponent* MyComp, class AActor* Other, class UPrimitiveComponent* OtherComp, bool bSelfMoved, FVector HitLocation, FVector HitNormal, FVector NormalImpulse, const FHitResult& Hit) override;

protected:
	void Boost(const FInputActionValue& InputActionValue);

public:
	void ClearPhysics();

public:
	void DamageToPlayer();

private:
	void ReboundPlayerBall();
};