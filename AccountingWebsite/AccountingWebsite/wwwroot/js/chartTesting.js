function GeneratePieChart(xLabels, yValues, xLabelCount)
{
	const presetColors = ["#FF1493", "#FF69B4", "#DC143C", "#4B0082", "#800080", "#EE82EE", "#006400", "#32CD32",
		"#FF4500", "#FF8C00", "#FFD700", "#FFDAB9", "#191970", "#4169E1", "#87CEEB", "#FFE4E1",
		"#CD853F", "#008080", "#00CED1", "#708090", "#C0C0C0"]
	function RandomColor()
	{
		if(presetColors.length === 0)
		{
			return "#" + (Math.random() * 0xFFFFFF << 0).toString(16);
		}

		const randomIndex = Math.floor(Math.random() * presetColors.length);
		const randomColor = presetColors.splice(randomIndex, 1)[0];

		return randomColor;
	}

	var colors = [];
	for(var i = 0; i < xLabelCount; i++)
	{
		colors[i] = RandomColor();
	}

	var ctx = document.getElementById("test-pie-chart").getContext('2d');
	var data = {
		labels: xLabels,
		datasets: [{
			label: "Fast Food Chart",
			borderWidth: 1,
			backgroundColor: colors,
			data: yValues
		}]
	};

	var options = {
		maintainAspectRatio: false,
		scales: {
			yAxes: [{
				ticks: {
					min: 0,
					beginAtZero: true
				},
				gridLines: {
					display: true,
					color: "rgba(255,99,164,0.2)"
				}
			}],
			xAxes: [{
				ticks: {
					min: 0,
					beginAtZero: true
				},
				gridLines: {
					display: false
				}
			}]
		},
		plugins: {
			tooltip: {
				callbacks: {
					label: function (context)
					{
						return context.label + ": $" + context.formattedValue;
					}
				}
			}
		}
	};

	var myChart = new Chart(ctx, {
		options: options,
		data: data,
		type: 'pie'
	});
}

function GenerateBarChart(xLabels, yValues, xLabelCount)
{
	const presetColors = ["#FF1493", "#FF69B4", "#DC143C", "#4B0082", "#800080", "#EE82EE", "#006400", "#32CD32",
		"#FF4500", "#FF8C00", "#FFD700", "#FFDAB9", "#191970", "#4169E1", "#87CEEB", "#FFE4E1",
		"#CD853F", "#008080", "#00CED1", "#708090", "#C0C0C0"]
	function RandomColor()
	{
		if(presetColors.length === 0)
		{
			return "#" + (Math.random() * 0xFFFFFF << 0).toString(16);
		}

		const randomIndex = Math.floor(Math.random() * presetColors.length);
		const randomColor = presetColors.splice(randomIndex, 1)[0];

		return randomColor;
	}

	var colors = [];
	for(var i = 0; i < xLabelCount; i++)
	{
		colors[i] = RandomColor();
	}

	var ctx = document.getElementById("test-bar-chart").getContext('2d');
	var data = {
		labels: xLabels,
		datasets: [{
			label: "Fast Food Chart",
			borderWidth: 1,
			backgroundColor: colors,
			data: yValues
		}]
	};

	var options = {
		maintainAspectRatio: false,
		scales: {
			yAxes: [{
				ticks: {
					min: 0,
					beginAtZero: true
				},
				gridLines: {
					display: true,
					color: "rgba(255,99,164,0.2)"
				}
			}],
			xAxes: [{
				ticks: {
					min: 0,
					beginAtZero: true
				},
				gridLines: {
					display: false
				}
			}]
		},
		plugins: {
			tooltip: {
				callbacks: {
					label: function (context)
					{
						return context.label + ": $" + context.formattedValue;
					}
				}
			}
		}
	};

	var myChart = new Chart(ctx, {
		options: options,
		data: data,
		type: 'bar'
	});
}