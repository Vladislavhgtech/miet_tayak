#include "stdafx.h"
#include "StateMachine.h"

bool State::StateMachine::IsDeterminate()
{
	for (auto element : stateType)
	{
		//https://github.com/pfultz2/Linq
		auto stateQueue = LINQ(
			from(w, stateType)
			where((w.CurrentState() == element.CurrentState()) && (w.ReadSymbol() == element.ReadSymbol()))
			select(w));
		if (boost::size(stateQueue) > 1)
			return false;
		/*auto states = stateType
			| linq::where([](list<StateType> x) { return x; })
			| linq::select([](StateType y) { return y; });
		StateType k = states.front();
		states.pop_back();
		vector<StateType> numbers = { stateType.front(), stateType.front()};
		auto r = numbers
			| linq::where([](StateType x) { return x; })
			| linq::select([](StateType x) { return x; });*/
	}
	return true;
}

bool State::StateMachine::workAutomat(string line)
{
	std::string lastState = "f0";

	std::string currentState = "q0";

	nextState = currentState;

	for (int i = 0; i < line.size(); i++) {
		if (!IsCorrectSymbolInLine(line[i])) {
			return false;
		}
	}

	std::string lastStatePattern = "(f{1})([0-9]+)";
	std::regex regexs(lastStatePattern);
	nextState = "f0";//допилить
	sregex_iterator endState(nextState.begin(), nextState.end(), regexs);
	auto endStateLast = std::sregex_iterator();
	bool endCurrentState = false;
	for (std::sregex_iterator i = endState; i != endStateLast; ++i) {
		std::smatch match = *i;
		std::string match_str = match.str();

		if (match_str._Equal(nextState))
			endCurrentState = true;
	}


	if (endCurrentState) {
		list<State::StateType> endStateQueue;

		std::list<State::StateType>::iterator it = stateType.begin();
		for (it; it != stateType.end(); ++it) {
			if (it->TransitionState() == nextState)
				endStateQueue.push_back(*it);
		}

		if (endStateQueue.size() == 0) {
			return false;
		}
		lastState = nextState;
		return true;
	}
	else
		return false;
	
	

	return false;
}

bool State::StateMachine::IsCorrectSymbolInLine(char lineSymbol)
{
	

	auto symbolQueue = 
		LINQ(
			from(w, stateType)
			where((char)(*w.ReadSymbol().c_str()) == lineSymbol)
			select(w)
		);
	auto stateQueue =
		LINQ(
			from(w, stateType)
			where(w.CurrentState() == nextState)
			select(w)
		);
	
	if (boost::size(symbolQueue) != 0 && boost::size(stateQueue) != 0) {
		std::list<State::StateType> stateModel;

		std::list<State::StateType>::iterator it = stateType.begin();
		for (it; it != stateType.end(); ++it) {
			if (stateModelQueue.empty()) {
				if (it->CurrentState()._Equal(nextState)
					&&
					(char)*it->ReadSymbol().c_str() == lineSymbol) 
				{
					stateModel.push_back((*it));//&(*it) чертов разыименователь указателей...
				}
			}
			else {
				std::list<State::StateType>::iterator its = stateModelQueue.begin();
				for (its; its != stateModelQueue.end(); ++its) {
					if (it->CurrentState()._Equal(its->TransitionState())
						&&
						(char)*it->ReadSymbol().c_str() == lineSymbol) 
					{
						stateModel.push_back((*it));//&(*it) чертов разыименователь указателей...
					}
				}
			}
		}

		if (!stateModel.empty())
		{
			stateModelQueue = stateModel;
		}
		else
			return false;
		return true;
	}
	else {
		return false;
	}
}

State::StateMachine::StateMachine()
{
	patternCurrentState = "(q)([0-9]*)?";
	patternTransitionState = "[=?]+[(q|f)?]([0-9]*)?";
	patternAnySymbol = patternCurrentState + "(.*?)" + patternTransitionState;

	//rulesStateParse = make_shared<regex>(patternCurrentState + "|" + patternAnySymbol + "|" + patternTransitionState);
	rulesStateParse = make_shared<regex>(patternCurrentState + "|" + patternTransitionState);
	automatSyntaxErrorFlag = false;
	determinateFlag = false;
}

State::StateType State::StateMachine::createNewState(string state)
{
	auto stateModel = new StateType();

	sregex_iterator words_begin(state.begin(), state.end(), *rulesStateParse);
	auto words_end = std::sregex_iterator();

	//iteratotrHelp = 0 - Current state, iteratorHelp = 1 - TransitionState, iteratorHelp = 2 - ReadSymbol
	string stateParse[3];
	int iteratorHelp = 0;
	for (std::sregex_iterator i = words_begin; i != words_end; ++i) {
		std::smatch match = *i;
		std::string match_str = match.str();
	
		stateParse[iteratorHelp] = match_str;
		iteratorHelp++;
		cout << "_" << match_str << " ";
	}

	cout << endl;

	char s = ' ';
	string newParseWith = "";
	for (string::iterator it = stateParse[1].begin(); it != stateParse[1].end(); ++it) {
		if (s == *it && s == '=') {
			stateParse[2] = *it;
		}
		else {
			newParseWith += *it;
		}
		s = *it;
	}
	stateParse[1] = newParseWith.c_str();

	//Проверяем правильно ли записанны состояния согласно плану
	bool flag = stateModel->isStateCorrect(stateParse[0], stateParse[1]);
	if (stateModel->isStateCorrect(stateParse[0], stateParse[1]) || iteratorHelp > 2) {
		cout << "Состояния не корректно написанны" << endl;
		automatSyntaxErrorFlag = true;
		return *stateModel;
	}

	//Символ обрабатываемый состоянием
	int first = state.find_first_of(',', sizeof(char)) + 1;
	int count = state.find_first_of('=', sizeof(char)) - first;
	if (!stateParse[2]._Equal("=")) {
		count = state.find_first_of('=', sizeof(char)) - first;
	}
	else {
		count = state.find_first_of('=', sizeof(char)) - first +1;
	}

	if (count != 1) {
		cout << "Введенно больше одного символа для одного состояния" << endl;
		automatSyntaxErrorFlag = true;
		return *stateModel;
	}
	stateParse[iteratorHelp] += state.substr(first, count);
	stateParse[1] = stateParse[1].substr(1, stateParse[1].size());
		
	stateModel->setState(stateParse[0], stateParse[1], stateParse[2]);

	return *stateModel;
}

list<State::StateType> State::StateMachine::getState()
{
	list<State::StateType> stateList;

	std::cout << "Введите путь к файлу с автоматом" << std::endl;
	std::string lineSymbol;
	std::getline(std::cin, lineSymbol);

	automatStream->open(lineSymbol);
	//(!automatStream->eof())
	while((!automatStream->eof())){
		
		string states = automatStream->readLine();
		if (states.empty())
			break;
		State::StateType state = createNewState(states);
		if (automatSyntaxErrorFlag)
			break;
		stateList.push_back(state);
	}
	automatStream->close();
	return stateList;
}

void State::StateMachine::proccesAutomat(string line)
{
	//Временное решение потом обдумаю как лучше
	automatStream = new Stream::StreamReader();

	stateType = getState();

	if (stateType.empty())
		cout << "Ошибка в автомате" << endl;

	if (!(determinateFlag = IsDeterminate()))
		cout << "Автомат недетерменирован" << endl;
	else
		cout << "Автомат детерменирован" << endl;

	bool work = workAutomat(line);

	if (work) {
		std::cout << "Автомат может работать с заданной строкой" << std::endl;
	}
	else {
		std::cout << "Автомат не может работать с заданной строкой.\nПроверьте либо строку, либо файл с автоматом" << std::endl;
	}
	std::cout << "Обработка завершена" << std::endl;
}

State::StateMachine::~StateMachine()
{
	if (!stateType.empty())
		stateType.clear();
	if (rulesStateParse != nullptr)
		rulesStateParse.reset();
	if (!patternAnySymbol.empty())
		patternAnySymbol.clear();
	if (!patternCurrentState.empty())
		patternCurrentState.clear();
	if (!patternTransitionState.empty())
		patternTransitionState.clear();
}
