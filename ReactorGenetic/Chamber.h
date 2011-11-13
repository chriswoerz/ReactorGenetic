#pragma once
#include "Reactor.h"

ref class Chamber
{
public:
	Chamber(Reactor^ &reactor);
	void OnPulseEvent(Reactor^ reactor, EventArgs^ args);	
private:	
	void HookupEvents();
};
