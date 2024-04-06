#pragma once

#include "CoreMinimal.h"
#include "ItemBase.h"
#include "Coin.generated.h"

UCLASS()
class ROLLINGBALL_API ACoin : public AItemBase
{
	GENERATED_BODY()

protected:
	void OnTouchedByPlayerBall() override;
};