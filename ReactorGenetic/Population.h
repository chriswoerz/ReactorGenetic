#pragma once
#include "Reactor.h"

ref class Population
{
private:
	array<Reactor^>^ reactors; 
	int populationCount;	
	int chamberPerReactorCount;
	int trialLength;
	bool InitPopulation(int populationSize, int chanberCount);
public:
	void Start();
	void Stop();
	void Pause();
	Population(void);
};
