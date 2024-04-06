#pragma once

#include "CoreMinimal.h"
#include "GameFramework/HUD.h"
#include "HUD_Title.generated.h"

UCLASS()
class ROLLINGBALL_API AHUD_Title : public AHUD
{
	GENERATED_BODY()

protected:
	UFUNCTION(BlueprintCallable, meta = (KeyWords = "set up hud title"))
	void Setup_HUD_Title(UUserWidget* TitleUserWidget);
};