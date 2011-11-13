#include "StdAfx.h"
#include "Population.h"
#include "Reactor.h"

Population::Population(void)
{
	populationCount = 100;
	chamberPerReactorCount = 2;
	trialLength = 10000;
	InitPopulation(populationCount, chamberPerReactorCount);
}

bool Population::InitPopulation(int populationSize, int chamberCount){
		this->reactors = gcnew array<Reactor^>(populationSize);
		for ( int p = 0; p < populationSize; p++){
			reactors[p] =   gcnew Reactor(chamberCount);
		}
		return true;
	}

void Population::Start(){
		for(int t = 0; t < trialLength; t++){
			for ( int r = 0; r < reactors->GetLength(0); r++){
				reactors[r]->RaisePulse();
			}
		}
	}
void Population::Stop(){
	
	}
void Population::Pause(){
	
	}
