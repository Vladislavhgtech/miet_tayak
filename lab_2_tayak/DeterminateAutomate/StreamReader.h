#pragma once
namespace Stream
{

	class StreamReader
	{
	private:
		fstream* fileStream;

		int _position;
		int _sizeFile;
	public:
		int getPosition() {
			return _position;
		}
		void setPosition(int position) {
			_position = position;

			if (fileStream->is_open())
				fileStream->seekg(position);
		}

		int getSizeFile() {
			return _sizeFile;
		}

		StreamReader();
		StreamReader(string path);

		void open(string path);
		void close();
		bool eof() {
			return fileStream->eof();
		}

		string readLine();
		char readByte();

		~StreamReader();
	};
}

