document.addEventListener('DOMContentLoaded', function () {
    // Date elements
    const checkInInput = document.querySelector('input[name="CheckInDate"]');
    const checkOutInput = document.querySelector('input[name="CheckOutDate"]');
    const roomRadios = document.querySelectorAll('input[name="RoomNumber"]');

    // Today's and tomorrow's dates
    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(tomorrow.getDate() + 1);

    // Format as YYYY-MM-DD
    const todayString = today.toISOString().split('T')[0];
    const tomorrowString = tomorrow.toISOString().split('T')[0];

    // Initialize dates
    if (checkInInput && !checkInInput.value) {
        checkInInput.value = todayString;
        checkInInput.min = todayString;
    }

    if (checkOutInput && !checkOutInput.value) {
        checkOutInput.value = tomorrowString;
        checkOutInput.min = tomorrowString;
    }

    // Update checkout min when checkin changes
    if (checkInInput) {
        checkInInput.addEventListener('change', function () {
            if (!this.value) return;

            // Set minimum checkout date
            const nextDay = new Date(this.value);
            nextDay.setDate(nextDay.getDate() + 1);
            const minDate = nextDay.toISOString().split('T')[0];

            checkOutInput.min = minDate;

            // Adjust checkout if needed
            if (new Date(checkOutInput.value) < nextDay) {
                checkOutInput.value = minDate;
            }

            validateDates();
        });
    }

    // Date validation function
    function validateDates() {
        if (!checkInInput.value || !checkOutInput.value) return;

        const checkIn = new Date(checkInInput.value);
        const checkOut = new Date(checkOutInput.value);

        // Clear previous errors
        checkInInput.classList.remove('is-invalid');
        checkOutInput.classList.remove('is-invalid');
        document.querySelectorAll('.date-error').forEach(e => e.remove());

        // Validate date order
        if (checkOut <= checkIn) {
            showDateError(checkOutInput, "Датата на напускане трябва да е след настаняването");
            return false;
        }

        return true;
    }

    // Show date error function
    function showDateError(input, message) {
        input.classList.add('is-invalid');
        const errorElement = document.createElement('div');
        errorElement.classList.add('date-error', 'text-danger', 'mt-1');
        errorElement.textContent = message;
        input.parentNode.appendChild(errorElement);
    }

    // Add validation to both date inputs
    [checkInInput, checkOutInput].forEach(input => {
        input?.addEventListener('change', validateDates);
    });

    // Initial validation
    validateDates();

    // Visual feedback styling
    const style = document.createElement('style');
    style.textContent = `
        .is-invalid {
            border-color: #dc3545 !important;
            box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25) !important;
        }
        .date-error {
            font-size: 0.875em;
        }
        input[type="date"]:focus {
            border-color: #80bdff;
            box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
        }
    `;
    document.head.appendChild(style);
});