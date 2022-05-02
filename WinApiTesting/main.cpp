

#include <Windows.h>

int main(int argc, char** argv)
{
	int a = sizeof(NONCLIENTMETRICSW);
	int b = sizeof(ICONMETRICSW);
	
	NONCLIENTMETRICSA metrics = { 0 };
	metrics.cbSize = sizeof(metrics);
	SystemParametersInfoA(SPI_GETNONCLIENTMETRICS, sizeof(NONCLIENTMETRICS), &metrics, NULL);
	return 0;
}