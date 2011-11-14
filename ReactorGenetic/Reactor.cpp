#include "StdAfx.h"
#include "Reactor.h"
#include "Chamber.h"

Reactor::Reactor(int chamberCount)
{
	this->chambers = gcnew array<Chamber^>(chamberCount);
	for(int i = 0; i < chamberCount; i++)
	{
		chambers[i] = gcnew Chamber();
		chambers[i]->HookupEvents();
	}
	hullHeat = 0;
}

int Reactor::GetWidth(){
	return this->chambers->GetLength(1) + 3;
}
int Reactor::GetHeight(){
	return 8;
}
int Reactor::GetCapacity(){
	return GetWidth()*GetHeight();
}
void Reactor::RaisePulse(){
	PulseEvent(this, gcnew EventArgs());
}