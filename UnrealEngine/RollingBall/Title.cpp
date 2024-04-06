#include "Title.h"
#include "Kismet/GameplayStatics.h"
#include "RollingBallGameInstance.h"
#include "Kismet/KismetSystemLibrary.h"

void UTitle::NativeConstruct()
{
	Super::NativeConstruct();

	if (PlayButton != nullptr) PlayButton->OnClicked.AddUniqueDynamic(this, &UTitle::OnClickedPlayButton);

	if (QuitButton != nullptr) QuitButton->OnClicked.AddUniqueDynamic(this, &UTitle::OnClickedQuitButton);
}

void UTitle::OnClickedPlayButton()
{
	URollingBallGameInstance* GameInstance = Cast<URollingBallGameInstance>(UGameplayStatics::GetGameInstance(GetWorld()));

	if (GameInstance != nullptr) GameInstance->ResetAllCounts();

	if (MainLevelName != NAME_None) UGameplayStatics::OpenLevel(GetWorld(), MainLevelName);
}

void UTitle::OnClickedQuitButton()
{
	APlayerController* PlayerController = UGameplayStatics::GetPlayerController(GetWorld(), 0);

	if (PlayerController != nullptr) UKismetSystemLibrary::QuitGame(GetWorld(), PlayerController, EQuitPreference::Quit, false);
}