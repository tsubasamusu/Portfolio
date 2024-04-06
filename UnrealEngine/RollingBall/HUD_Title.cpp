#include "HUD_Title.h"
#include "Kismet/GameplayStatics.h"
#include "Blueprint/WidgetBlueprintLibrary.h"
#include "Kismet/KismetSystemLibrary.h"

void AHUD_Title::Setup_HUD_Title(UUserWidget* TitleUserWidget)
{
	if (TitleUserWidget == nullptr) return;

	TitleUserWidget->AddToViewport(0);

	APlayerController* PlayerController = UGameplayStatics::GetPlayerController(GetWorld(), 0);

	if (PlayerController == nullptr) return;

	UWidgetBlueprintLibrary::SetInputMode_GameAndUIEx(PlayerController, TitleUserWidget, EMouseLockMode::DoNotLock, true, false);

	PlayerController->SetShowMouseCursor(true);
}