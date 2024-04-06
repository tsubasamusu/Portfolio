#include "Status.h"
#include "Kismet/GameplayStatics.h"
#include "RollingBallGameInstance.h"

void UStatus::NativeConstruct()
{
	Super::NativeConstruct();
}

bool UStatus::Initialize()
{
	bool  bSucceededToInitializeParentClass = Super::Initialize();

	if (!bSucceededToInitializeParentClass) return false;

	CurrentPlayerHealthPointText->TextDelegate.BindUFunction(this, "SetCurrentPlayerHealthPointText");

	GotCoinsCountText->TextDelegate.BindUFunction(this, "SetGotCoinsCountText");

	return true;
}

FText UStatus::SetCurrentPlayerHealthPointText()
{
	URollingBallGameInstance* GameInstance = Cast<URollingBallGameInstance>(UGameplayStatics::GetGameInstance(GetWorld()));

	if (GameInstance == nullptr) return FText();

	return FText::FromString(FString::FromInt(GameInstance->GetCurrentPlayerHealthPoint()));
}

FText UStatus::SetGotCoinsCountText()
{
	URollingBallGameInstance* GameInstance = Cast<URollingBallGameInstance>(UGameplayStatics::GetGameInstance(GetWorld()));

	if (GameInstance == nullptr) return FText();

	return FText::FromString(FString::FromInt(GameInstance->GetGotCoinsCount()));
}