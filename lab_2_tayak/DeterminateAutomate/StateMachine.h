#pragma once
#include "StreamReader.h"
namespace State
{
	class StateType 
	{
	private:
		string _transitionState;
		string _currentState;
		string _readSymbol;
	public:

		const string& CurrentState() const { return _currentState; }
		const string& ReadSymbol() const { return _readSymbol; }
		const string& TransitionState() const { return _transitionState; }
		void setState(
			string currentState, 
			string transitionState, 
			string readSymbol) {
			_currentState = currentState;
			_transitionState = transitionState;
			_readSymbol = readSymbol;
		}

		StateType() 
		{

		}
		~StateType() 
		{
			if (_currentState.empty())
				_currentState.clear();
			if (_transitionState.empty())
				_transitionState.clear();
			if (_readSymbol.empty())
				_readSymbol.clear();
		}

		bool isStateCorrect(string current, string trans) {
			//Check currentState
			char buf = ' ';
			for (string::iterator it = current.begin(); it != current.end(); ++it) {
				if (buf != ' ') {
					if (*it == buf)
						return false;
				}
				buf = *it;
			}
			//Check transState
			buf = ' ';
			for (string::iterator it = trans.begin(); it != trans.end(); ++it) {
				if (buf != ' ') {
					if (*it == buf)
						return false;
				}
				buf = *it;
			}
			return false;
		}

		//Получаем текущую строку
		string getState() 
		{
			if (!_currentState.empty() && !_transitionState.empty() && !_readSymbol.empty())
			{
				string resultState =
					"First state: " + _currentState +
					"\nSymbol: " + _readSymbol +
					"\nLast state: " + _transitionState;
				return resultState;
			}
			return "Empty";
		}
	};

	class StateMachine
	{
	private:
		Stream::StreamReader* automatStream;

		bool determinateFlag;
		bool automatSyntaxErrorFlag;
		list<StateType> stateType;
		shared_ptr<regex> rulesStateParse;

		string nextState;
		std::list<State::StateType> stateModelQueue;

		string patternCurrentState = "[q?]+([0-9]*)?";
		string patternTransitionState = "[=?]+[(q|f)?]+([0-9]*)?";
		string patternAnySymbol = "";

		//Проверка на детерменированность автомата
		bool IsDeterminate();
		bool workAutomat(string line);
		bool IsCorrectSymbolInLine(char lineSymbol);
	public:
		StateMachine();
		~StateMachine();


		//Создаем новое состояние
		State::StateType createNewState(string state);
		//Получение автомата как словаря map
		list<StateType> getState();
		void proccesAutomat(string line);
	};
}


