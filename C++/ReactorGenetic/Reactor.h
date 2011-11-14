#pragma once
#include "Chamber.h"
using namespace System;
using namespace System::Threading;

ref class Reactor
{
private:
	int hullHeat;	
public:
	Reactor();
	event EventHandler^ PulseEvent;
	int GetWidth();
	int GetHeight();
	int GetCapacity();
	void RaisePulse();
};
