#include "HUD_Main.h"
#include "Blueprint/WidgetBlueprintLibrary.h"

void AHUD_Main::Setup_HUD_Main(UUserWidget* MainUserWidget)
{
	if (MainUserWidget != nullptr) MainUserWidget->AddToViewport(0);
}