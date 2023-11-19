function SetUpCategoryTwoOptions(catOneId, catTwoId, defaultOptionText, categoryOptions)
{
	$("#" + catOneId).change(function ()
	{
		// Remove the existing options
		$("#" + catTwoId).find('option').remove();

		// Get the new ones
		var catTwoOptions = categoryOptions[this.value];

		// Add the default
		var defaultOption = document.createElement('option');
		defaultOption.text = defaultOptionText;
		defaultOption.value = defaultOptionText;
		$("#" + catTwoId).append(defaultOption);

		// Check if there are any others to add
		if(catTwoOptions === undefined)
		{
			return;
		}

		// Add the others
		for(var i = 0; i < catTwoOptions.length; i++)
		{
			var option = document.createElement('option');
			option.text = catTwoOptions[i];
			option.value = catTwoOptions[i];
			$("#" + catTwoId).append(option);
		}
	});

	// Initialize the options
	$("#" + catOneId).trigger("change");
}

function SelectOption(id, valueToSelect)
{
	let dropdown = document.getElementById(id);
	dropdown.value = valueToSelect;
}

function OnTimeframeSelect()
{
	if($("#time-frame-dropdown").val() == "Custom")
	{
		$("#customDateSelection").css("display", "block");
	}

	else
	{
		$("#customDateSelection").css("display", "none");
	}
}

function LoadReport()
{
	$.ajax
	({
        type: "POST",
		url: "/Reports/LoadGraphs",
		data: $("#reportCriteria").serialize(),
		success: function (value)
		{
			if(value) 
			{
				$("#reportContainer").empty();
				$("#reportContainer").append(value);

				LoadTableContents();
			}
		},
	});
}

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

	var ctx = document.getElementById("pie-chart").getContext('2d');
	var data = {
		labels: xLabels,
		datasets: [{
			label: "Pie Chart",
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

	var ctx = document.getElementById("bar-chart").getContext('2d');
	var data = {
		labels: xLabels,
		datasets: [{
			label: "Bar Chart",
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
			},
			legend: {
				display: false,
			}
		}
	};

	var myChart = new Chart(ctx, {
		options: options,
		data: data,
		type: 'bar'
	});
}