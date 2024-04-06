#include "PlayerBall.h"
#include "Camera/CameraComponent.h"
#include "GameFramework/SpringArmComponent.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"
#include "InputMappingContext.h"
#include "InputActionValue.h"
#include "Kismet/GameplayStatics.h"
#include "RollingBallGameInstance.h"
#include "GameMode_Main.h"

void APlayerBall::SetupPlayerBall(UStaticMeshComponent* StaticMeshComponent, USpringArmComponent* SpringArmComponent, UCameraComponent* CameraComponent)
{
	SphereStaticMesh = StaticMeshComponent;

	SpringArm = SpringArmComponent;

	Camera = CameraComponent;

	SetupComponentSettings();

	SetupEnhancedInputMappingContext();
}

void APlayerBall::SetupComponentSettings()
{
	if (SphereStaticMesh != nullptr)
	{
		SphereStaticMesh->SetSimulatePhysics(true);

		SphereStaticMesh->SetCollisionProfileName("PhysicsActor");

		SphereStaticMesh->BodyInstance.bNotifyRigidBodyCollision = true;
	}

	if (Camera != nullptr) Camera->PostProcessSettings.MotionBlurAmount = bEnableMotionBlurForCamera ? 1.0f : 0.0f;

	if (SpringArm != nullptr)
	{
		SpringArm->bUsePawnControlRotation = true;

		SpringArm->bInheritPitch = SpringArm->bInheritRoll = SpringArm->bInheritYaw = true;
	}
}

void APlayerBall::SetupEnhancedInputMappingContext()
{
	const APlayerController* PlayerController = Cast<APlayerController>(Controller);

	if (PlayerController == nullptr) return;

	UEnhancedInputLocalPlayerSubsystem* EnhancedInputLocalPlayerSubsystem = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(PlayerController->GetLocalPlayer());
	
	if (EnhancedInputLocalPlayerSubsystem != nullptr) EnhancedInputLocalPlayerSubsystem->AddMappingContext(DefaultInputMappingContext, 0);
}

void APlayerBall::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);

	UEnhancedInputComponent* EnhancedInputComponent = Cast<UEnhancedInputComponent>(PlayerInputComponent);

	if (EnhancedInputComponent == nullptr) return;

	if (MovementInputAction != nullptr) EnhancedInputComponent->BindAction(MovementInputAction, ETriggerEvent::Triggered, this, &APlayerBall::ControlBallMovement);

	if (CameraInputAction != nullptr) EnhancedInputComponent->BindAction(CameraInputAction, ETriggerEvent::Triggered, this, &APlayerBall::ControlLooking);

	if (JumpInputAction != nullptr) EnhancedInputComponent->BindAction(JumpInputAction, ETriggerEvent::Triggered, this, &APlayerBall::Jump);

	if (BoostInputAction != nullptr) EnhancedInputComponent->BindAction(BoostInputAction, ETriggerEvent::Triggered, this, &APlayerBall::Boost);
}

void APlayerBall::ControlBallMovement(const FInputActionValue& InputActionValue)
{
	const FVector2D InputVelocity = InputActionValue.Get<FVector2D>();

	FVector ForceToAdd = FVector(InputVelocity.Y, InputVelocity.X, 0.0f) * MovingSpeed;

	if (Camera != nullptr) ForceToAdd = Camera->GetComponentToWorld().TransformVectorNoScale(ForceToAdd);

	if (SphereStaticMesh != nullptr) SphereStaticMesh->AddForce(ForceToAdd, NAME_None, true);
}

void APlayerBall::ControlLooking(const FInputActionValue& InputActionValue)
{
	FVector2D InputVelocity = InputActionValue.Get<FVector2D>();

	if (Controller == nullptr) return;

	AddControllerYawInput(InputVelocity.X);

	AddControllerPitchInput(InputVelocity.Y);

	double LimitedPitchAngle = FMath::ClampAngle(GetControlRotation().Pitch, -MaxCameraAngle, MaxCameraAngle);

	APlayerController* PlayerController = UGameplayStatics::GetPlayerController(this, 0);

	if (PlayerController != nullptr) PlayerController->SetControlRotation(FRotator(LimitedPitchAngle, GetControlRotation().Yaw, 0.0f));
}

void APlayerBall::Jump(const FInputActionValue& InputActionValue)
{
	if (!InputActionValue.Get<bool>() || !bCanJump) return;

	SphereStaticMesh->AddImpulse(FVector(0.0f, 0.0f, JumpImpluse), NAME_None, true);

	bCanJump = false;
}

void APlayerBall::NotifyHit(class UPrimitiveComponent* MyComp, class AActor* Other, class UPrimitiveComponent* OtherComp, bool bSelfMoved, FVector HitLocation, FVector HitNormal, FVector NormalImpulse, const FHitResult& Hit)
{
	Super::NotifyHit(MyComp, Other, OtherComp, bSelfMoved, HitLocation, HitNormal, NormalImpulse, Hit);

	bCanJump = true;
}

void APlayerBall::Boost(const FInputActionValue& InputActionValue)
{
	if (!InputActionValue.Get<bool>()) return;

	FVector ForwardVector = Camera->GetForwardVector().GetSafeNormal(0.0001f);

	FVector AdditionalSpeedVector = FVector(ForwardVector.Y * AdditionalSpeed * -1.0f, ForwardVector.X * AdditionalSpeed, 0.0f);

	SphereStaticMesh->AddTorqueInRadians(AdditionalSpeedVector, NAME_None, true);
}

void APlayerBall::ClearPhysics()
{
	if (SphereStaticMesh == nullptr) return;

	SphereStaticMesh->SetSimulatePhysics(false);

	SphereStaticMesh->SetSimulatePhysics(true);
}

void APlayerBall::DamageToPlayer()
{
	ReboundPlayerBall();

	URollingBallGameInstance* GameInstance = Cast<URollingBallGameInstance>(UGameplayStatics::GetGameInstance(GetWorld()));

	if (GameInstance == nullptr) return;

	GameInstance->DecreasePlayerHealthPoint();

	if (GameInstance->GetCurrentPlayerHealthPoint() > 0) return;

	AGameMode_Main* GameMode = Cast<AGameMode_Main>(UGameplayStatics::GetGameMode(GetWorld()));

	if (GameMode != nullptr) GameMode->RespawnPlayer(this);
}

void APlayerBall::ReboundPlayerBall()
{
	if (Camera == nullptr) return;

	FVector ImpluseVector = Camera->GetForwardVector() * (-1.0f * ReboundPower);

	if (SphereStaticMesh != nullptr) SphereStaticMesh->AddImpulse(ImpluseVector, NAME_None, true);
}