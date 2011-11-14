#include "StdAfx.h"
#include "Chamber.h"
#include "Reactor.h"

Chamber::Chamber(void)
{
	HookupEvents();
}

//void Chamber::OnPulseEvent(Reactor^ reactor, EventArgs^ args){
//	
//}

void Chamber::HookupEvents(){
	//ItsReactor->PulseEvent += gcnew EventHandler(this, &EventReciever::OnPulseEvent);
}
