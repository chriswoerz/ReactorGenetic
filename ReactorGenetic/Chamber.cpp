#include "StdAfx.h"
#include "Chamber.h"
#include "Reactor.h"

Chamber::Chamber(Reactor^ &reactor)
{
	HookupEvents();
}

void Chamber::OnPulseEvent(Reactor^ reactor, EventArgs^ args){
	
}

Chamber::HookupEvents(){
	ItsReactor->PulseEvent += gcnew EventHandler(this, &EventReciever::OnPulseEvent);
}
