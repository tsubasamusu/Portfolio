#pragma once

#include "Components/Button.h"
#include "CoreMinimal.h"
#include "Blueprint/UserWidget.h"
#include "Title.generated.h"

UCLASS()
class ROLLINGBALL_API UTitle : public UUserWidget
{
	GENERATED_BODY()

private:
	UPROPERTY(EditAnywhere)
	FName MainLevelName;

private:
	UPROPERTY(meta = (BindWidget))
	TObjectPtr<UButton> PlayButton;

private:
	UPROPERTY(meta = (BindWidget))
	TObjectPtr<UButton> QuitButton;

protected:
	void NativeConstruct() override;

private:
	UFUNCTION()
	void OnClickedPlayButton();

private:
	UFUNCTION()
	void OnClickedQuitButton();
};