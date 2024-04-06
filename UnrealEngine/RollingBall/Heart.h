#pragma once

#include "CoreMinimal.h"
#include "ItemBase.h"
#include "Heart.generated.h"

UCLASS()
class ROLLINGBALL_API AHeart : public AItemBase
{
	GENERATED_BODY()

protected:
	void OnTouchedByPlayerBall() override;
};