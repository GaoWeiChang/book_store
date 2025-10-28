function IncreaseCartItem(url) {
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            if (data.success) {
                let id = url.split('/').pop();
                let countElement = $('#count-' + id);
                let currentCount = parseInt(countElement.text());
                let newCount = currentCount + 1;

                // updatet count
                countElement.text(newCount)

                // update total price
                let priceElement = $('#price-' + id);
                let priceText = priceElement.text().trim();
                let price = parseFloat(priceText.replace(/[$,]/g, ''));
                let newTotal = price * newCount;

                $('#total-' + id).text('$' + newTotal.toFixed(2));
                updateOrderTotal();
            }
        }
    });
}

function DecreaseCartItem(url) {
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            if (data.success) {
                let id = url.split('/').pop();
                let countElement = $('#count-' + id);
                let currentCount = parseInt(countElement.text());

                // update count
                if (currentCount <= 1) {
                    $('#row-' + id).remove();
                } else {
                    let newCount = currentCount - 1;
                    countElement.text(newCount);

                    let priceElement = $('#price-' + id);
                    let priceText = priceElement.text().trim();
                    let price = parseFloat(priceText.replace(/[$,]/g, ''));
                    let newTotal = price * newCount;

                    $('#total-' + id).text('$' + newTotal.toFixed(2));
                }

                updateOrderTotal();
            }
        }
    });
}

function updateOrderTotal() {
    let total = 0;

    $('[id^="total-"]').each(function () {
        let amountText = $(this).text().trim();
        // ลบ $ และ , ออกก่อนแปลงเป็นตัวเลข
        let amount = parseFloat(amountText.replace(/[$,]/g, ''));
        if (!isNaN(amount)) {
            total += amount;
        }
    });

    // Format แบบมี $ และคอมม่าถ้าเกิน 1000
    $('#order-total').text('$' + total.toLocaleString('en-US', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    }));
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    let id = url.split('/').pop();
                    $('#row-' + id).remove();
                    toastr.success(data.message);
                }
            })
        }
    });
}