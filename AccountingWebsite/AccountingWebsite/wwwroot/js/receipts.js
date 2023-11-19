$('#carouselReceiptsToDisplay').on('slide.bs.carousel', function onSlide(ev)
{
	LoadReceiptDetails($("#receiptImage" + ev.to).attr('src').replace("/receipt_images/", ""))
});

function LoadReceiptDetails(fileName)
{
	var formData = new FormData();
	formData.append("fileName", fileName);

	$.ajax
	({
		url: "/Receipts/FindReceiptDetails",
		data: formData,
		processData: false,
		contentType: false,
		type: "POST",
		success: function (value)
		{
			if(value) 
			{
				$("#receiptDetails div").remove();
				$("#receiptDetails").append(value);
			}
		},
	});
}