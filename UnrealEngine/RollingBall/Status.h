#pragma once

#include "Components/TextBlock.h"
#include "CoreMinimal.h"
#include "Blueprint/UserWidget.h"
#include "Status.generated.h"

UCLASS()
class ROLLINGBALL_API UStatus : public UUserWidget
{
	GENERATED_BODY()

private:
	UPROPERTY(meta = (BindWidget))
	TObjectPtr<UTextBlock> CurrentPlayerHealthPointText;

private:
	UPROPERTY(meta = (BindWidget))
	TObjectPtr<UTextBlock> GotCoinsCountText;

protected:
	virtual void NativeConstruct() override;

protected:
	bool Initialize() override;

private:
	UFUNCTION()
	FText SetCurrentPlayerHealthPointText();

private:
	UFUNCTION()
	FText SetGotCoinsCountText();
};