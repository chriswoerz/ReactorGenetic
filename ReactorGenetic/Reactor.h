#pragma once
#include "Chamber.h"
using namespace System;
using namespace System::Threading;

ref class Reactor
{
private:
	int hullHeat;	
	array<Chamber^>^ chambers;
public:
	Reactor(int chamberCount);
	event EventHandler^ PulseEvent;
	int GetWidth();
	int GetHeight();
	int GetCapacity();
	void RaisePulse();
};
