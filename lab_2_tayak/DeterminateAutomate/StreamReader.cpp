#include "stdafx.h"
#include "StreamReader.h"

Stream::StreamReader::StreamReader()
{
}

Stream::StreamReader::StreamReader(string path)
{
	fileStream = new fstream(path.c_str(), ios::binary | ios::in | ios::ate);

	_sizeFile = fileStream->tellg();

	setPosition(0);
}

void Stream::StreamReader::open(string path)
{
	if (fileStream == nullptr) {
		fileStream = new fstream(path.c_str(), ios::binary | ios::in | ios::ate);

		_sizeFile = fileStream->tellg();

		setPosition(0);
	}
}

string Stream::StreamReader::readLine()
{
	string line;

	getline(*fileStream, line);
	
	_position = fileStream->tellg();
	
	return line;
}

char Stream::StreamReader::readByte()
{
	char buff;

	fileStream->read(&buff, sizeof(char));
	_position = fileStream->tellg();

	return buff;
}

void Stream::StreamReader::close()
{
	if (fileStream->is_open())
		fileStream->close();
}

Stream::StreamReader::~StreamReader()
{
	if (fileStream != nullptr)
		delete fileStream;
}
