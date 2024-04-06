#include "ItemBase.h"
#include "PlayerBall.h"
#include "Components/SphereComponent.h"
#include "GameFramework/RotatingMovementComponent.h"
#include "Kismet/KismetSystemLibrary.h"

AItemBase::AItemBase()
{
	{
		StaticMesh = CreateDefaultSubobject<UStaticMeshComponent>("StaticMeshComponent");

		RootComponent = StaticMesh;
	}

	{
		Collision = CreateDefaultSubobject<USphereComponent>("SphereComponent");

		Collision->SetupAttachment(RootComponent);

		Collision->SetCollisionProfileName("OverlapAllDynamic");
	}

	{
		RotatingMovementComponent = CreateDefaultSubobject<URotatingMovementComponent>("RotatingMovementComponent");
	
		RotatingMovementComponent->SetUpdatedComponent(RootComponent);
	}
}

void AItemBase::OnCollisionBeginOverlap(AActor* OtherActor)
{
	if (Cast<APlayerBall>(OtherActor) == nullptr) return;

	OnTouchedByPlayerBall();

	this->Destroy();
}

void AItemBase::OnTouchedByPlayerBall()
{

}