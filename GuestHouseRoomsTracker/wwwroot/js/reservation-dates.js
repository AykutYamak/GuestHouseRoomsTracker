// Date validation script for reservation form
document.addEventListener('DOMContentLoaded', function () {
    const checkInInput = document.querySelector('input[name="CheckInDate"]');
    const checkOutInput = document.querySelector('input[name="CheckOutDate"]');

    // Get today's date in YYYY-MM-DD format
    const today = new Date();
    const todayString = today.toISOString().split('T')[0];

    // Set minimum date to today for both inputs
    if (checkInInput) {
        checkInInput.setAttribute('min', todayString);

        // Add event listener for check-in date changes
        checkInInput.addEventListener('change', function () {
            const selectedCheckIn = this.value;

            // Validate that check-in is not in the past
            if (selectedCheckIn < todayString) {
                this.value = '';
                showDateError('Датата на настаняване не може да бъде в миналото!');
                return;
            }

            // Update minimum check-out date to be at least the check-in date
            if (checkOutInput) {
                checkOutInput.setAttribute('min', selectedCheckIn);

                // Clear check-out if it's before the new check-in date
                if (checkOutInput.value && checkOutInput.value < selectedCheckIn) {
                    checkOutInput.value = '';
                    showDateError('Датата на напускане трябва да бъде след датата на настаняване!');
                }
            }

            hideConflictWarning();
        });
    }

    // Set minimum date for check-out and add validation
    if (checkOutInput) {
        checkOutInput.setAttribute('min', todayString);

        checkOutInput.addEventListener('change', function () {
            const selectedCheckOut = this.value;
            const selectedCheckIn = checkInInput ? checkInInput.value : '';

            // Validate that check-out is not in the past
            if (selectedCheckOut < todayString) {
                this.value = '';
                showDateError('Датата на напускане не може да бъде в миналото!');
                return;
            }

            // Validate that check-out is after check-in
            if (selectedCheckIn && selectedCheckOut <= selectedCheckIn) {
                this.value = '';
                showDateError('Датата на напускане трябва да бъде след датата на настаняване!');
                return;
            }

            hideConflictWarning();
        });
    }

    // Function to show date error messages
    function showDateError(message) {
        // Remove any existing error alerts
        const existingAlert = document.querySelector('.date-error-alert');
        if (existingAlert) {
            existingAlert.remove();
        }

        // Create new error alert
        const alertDiv = document.createElement('div');
        alertDiv.className = 'alert alert-danger date-error-alert mt-2';
        alertDiv.innerHTML = `
            <i class="fas fa-exclamation-circle me-2"></i>
            ${message}
        `;

        // Insert after the date inputs row
        const dateRow = document.querySelector('.row.mt-2');
        if (dateRow) {
            dateRow.parentNode.insertBefore(alertDiv, dateRow.nextSibling);
        }

        // Auto-hide after 5 seconds
        setTimeout(() => {
            if (alertDiv && alertDiv.parentNode) {
                alertDiv.remove();
            }
        }, 5000);
    }

    // Function to hide conflict warning (existing functionality)
    function hideConflictWarning() {
        const conflictWarning = document.querySelector('.conflict-warning');
        if (conflictWarning && !conflictWarning.classList.contains('d-none')) {
            conflictWarning.classList.add('d-none');
        }
    }

    // Additional CSS for better visual feedback
    const style = document.createElement('style');
    style.textContent = `
        /* Style for date inputs with restrictions */
        input[type="date"]:invalid {
            border-color: #dc3545;
            box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25);
        }
        
        /* Custom error alert styling */
        .date-error-alert {
            border-left: 4px solid #dc3545;
            animation: slideIn 0.3s ease-out;
        }
        
        @keyframes slideIn {
            from {
                opacity: 0;
                transform: translateY(-10px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }
        
        /* Disable past dates visually (browser dependent) */
        input[type="date"]::-webkit-calendar-picker-indicator {
            cursor: pointer;
        }
    `;
    document.head.appendChild(style);

    // Form submission validation
    const form = document.querySelector('form');
    if (form) {
        form.addEventListener('submit', function (e) {
            const checkInValue = checkInInput ? checkInInput.value : '';
            const checkOutValue = checkOutInput ? checkOutInput.value : '';

            // Final validation before submission
            if (checkInValue < todayString) {
                e.preventDefault();
                showDateError('Датата на настаняване не може да бъде в миналото!');
                checkInInput.focus();
                return false;
            }

            if (checkOutValue < todayString) {
                e.preventDefault();
                showDateError('Датата на напускане не може да бъде в миналото!');
                checkOutInput.focus();
                return false;
            }

            if (checkInValue && checkOutValue && checkOutValue <= checkInValue) {
                e.preventDefault();
                showDateError('Датата на напускане трябва да бъде след датата на настаняване!');
                checkOutInput.focus();
                return false;
            }
        });
    }
});