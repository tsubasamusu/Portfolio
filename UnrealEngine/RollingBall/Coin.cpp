#include "Coin.h"
#include "Kismet/GameplayStatics.h"
#include "RollingBallGameInstance.h"

void ACoin::OnTouchedByPlayerBall()
{
	Super::OnTouchedByPlayerBall();

	URollingBallGameInstance* GameInstance = Cast<URollingBallGameInstance>(UGameplayStatics::GetGameInstance(GetWorld()));

	if (GameInstance != nullptr) GameInstance->AddGotCoinsCount();
}