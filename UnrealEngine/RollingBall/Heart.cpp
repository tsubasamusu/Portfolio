#include "Heart.h"
#include "Kismet/GameplayStatics.h"
#include "RollingBallGameInstance.h"

void AHeart::OnTouchedByPlayerBall()
{
	Super::OnTouchedByPlayerBall();

	URollingBallGameInstance* GameInstance = Cast<URollingBallGameInstance>(UGameplayStatics::GetGameInstance(GetWorld()));

	if (GameInstance != nullptr) GameInstance->AddPlayerHealthPoint();
}