#include <iostream>

class Thing {
public:
	Thing() {
		std::cout << "  [ Thing is created! ]" << std::endl;
	}

	~Thing() {
		std::cout << "  [ Thing is destroyed! ]" << std::endl;
	}
};

int main() {
	std::cout << "Start of scope..." << std::endl;

	std::cout << "{" << std::endl;
	{
		std::cout << "  First line of scope..." << std::endl;
	
		Thing t1;
	
		std::cout << "  Last line of scope..." << std::endl;
	}
	std::cout << "}" << std::endl;
	
	std::cout << "End of scope..." << std::endl;
}

