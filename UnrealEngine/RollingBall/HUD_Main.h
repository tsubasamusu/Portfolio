#pragma once

#include "CoreMinimal.h"
#include "GameFramework/HUD.h"
#include "HUD_Main.generated.h"

UCLASS()
class ROLLINGBALL_API AHUD_Main : public AHUD
{
	GENERATED_BODY()

protected:
	UFUNCTION(BlueprintCallable, meta = (KeyWords = "set up hud main"))
	void Setup_HUD_Main(UUserWidget* MainUserWidget);
};