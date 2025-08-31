// Dashboard JavaScript Functions

// Toggle user dropdown menu
function toggleDropdown() {
    const dropdown = document.getElementById('userDropdown');
    if (dropdown) {
        dropdown.classList.toggle('show');
    }
}

// Close dropdown when clicking outside
document.addEventListener('click', function (event) {
    const dropdown = document.getElementById('userDropdown');
    const dropdownBtn = document.querySelector('.dropdown-btn');

    if (dropdown && !event.target.closest('.dropdown') && !event.target.closest('.dropdown-btn')) {
        dropdown.classList.remove('show');
    }
});

// Mobile sidebar toggle
function toggleSidebar() {
    const sidebar = document.getElementById('sidebar');
    if (sidebar) {
        sidebar.classList.toggle('show');
    }
}

// Toggle submenu in sidebar
function toggleSubmenu(element) {
    event.preventDefault();
    const parentItem = element.parentElement;
    const submenu = parentItem.querySelector('.submenu');

    // Close other open submenus
    document.querySelectorAll('.nav-item.has-submenu').forEach(item => {
        if (item !== parentItem) {
            item.classList.remove('active');
            const otherSubmenu = item.querySelector('.submenu');
            if (otherSubmenu) {
                otherSubmenu.classList.remove('active');
            }
        }
    });

    // Toggle current submenu
    parentItem.classList.toggle('active');
    if (submenu) {
        submenu.classList.toggle('active');
    }
}

// Toggle submenu in dashboard cards
function toggleSubmenuCard(cardId) {
    event.stopPropagation();
    const submenu = document.getElementById(cardId + '-submenu');
    const allSubmenus = document.querySelectorAll('.card-submenu');

    // Close other submenus
    allSubmenus.forEach(menu => {
        if (menu !== submenu) {
            menu.classList.remove('active');
        }
    });

    // Toggle current submenu
    if (submenu) {
        submenu.classList.toggle('active');
    }
}

// Show coming soon notification
function showComingSoon(feature) {
    showNotification(`${feature} estará disponible próximamente`, 'info');
}

// Close card submenus when clicking outside
document.addEventListener('click', function (event) {
    if (!event.target.closest('.dashboard-card')) {
        document.querySelectorAll('.card-submenu').forEach(submenu => {
            submenu.classList.remove('active');
        });
    }
});

// Add active class to current navigation item
document.addEventListener('DOMContentLoaded', function () {
    // Get current path
    const currentPath = window.location.pathname;
    const navItems = document.querySelectorAll('.nav-item a');

    navItems.forEach(item => {
        const parent = item.parentElement;
        // Remove active class from all items
        parent.classList.remove('active');

        // Add active class to current item
        if (item.getAttribute('href') === currentPath) {
            parent.classList.add('active');
        }
    });

    // Add hover effects to navigation items
    document.querySelectorAll('.nav-item').forEach(item => {
        item.addEventListener('mouseenter', function () {
            if (!this.classList.contains('active')) {
                this.style.background = 'rgba(255, 255, 255, 0.1)';
            }
        });

        item.addEventListener('mouseleave', function () {
            if (!this.classList.contains('active')) {
                this.style.background = 'transparent';
            }
        });
    });

    // Add entrance animations to dashboard cards
    const cards = document.querySelectorAll('.dashboard-card');
    cards.forEach((card, index) => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(30px)';

        setTimeout(() => {
            card.style.transition = 'all 0.6s ease';
            card.style.opacity = '1';
            card.style.transform = 'translateY(0)';
        }, index * 100);
    });

    // Add entrance animations to stat cards
    const statCards = document.querySelectorAll('.stat-card');
    statCards.forEach((card, index) => {
        card.style.opacity = '0';
        card.style.transform = 'translateX(-30px)';

        setTimeout(() => {
            card.style.transition = 'all 0.6s ease';
            card.style.opacity = '1';
            card.style.transform = 'translateX(0)';
        }, 600 + (index * 100));
    });
});

// Smooth scroll for navigation links
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});

// Handle card clicks with loading effect
function handleCardClick(element) {
    // Add loading effect
    element.style.transform = 'scale(0.95)';
    element.style.opacity = '0.7';

    setTimeout(() => {
        element.style.transform = 'scale(1)';
        element.style.opacity = '1';
    }, 150);
}

// Add click handlers to all dashboard cards
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.dashboard-card').forEach(card => {
        card.addEventListener('click', function () {
            handleCardClick(this);
        });
    });
});

// Notification system
function showNotification(message, type = 'info', duration = 3000) {
    // Create notification element
    const notification = document.createElement('div');
    notification.className = `notification notification-${type}`;
    notification.innerHTML = `
        <i class="fas fa-${type === 'success' ? 'check-circle' : type === 'error' ? 'exclamation-circle' : 'info-circle'}"></i>
        <span>${message}</span>
        <button onclick="this.parentElement.remove()" class="notification-close">
            <i class="fas fa-times"></i>
        </button>
    `;

    // Add styles
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background: white;
        padding: 15px 20px;
        border-radius: 10px;
        box-shadow: 0 10px 30px rgba(0,0,0,0.1);
        border-left: 4px solid ${type === 'success' ? '#28a745' : type === 'error' ? '#dc3545' : '#17a2b8'};
        display: flex;
        align-items: center;
        gap: 10px;
        z-index: 9999;
        animation: slideInRight 0.3s ease;
        max-width: 400px;
    `;

    // Add to document
    document.body.appendChild(notification);

    // Auto remove after duration
    setTimeout(() => {
        notification.style.animation = 'slideOutRight 0.3s ease';
        setTimeout(() => {
            if (notification.parentElement) {
                notification.remove();
            }
        }, 300);
    }, duration);
}

// Add notification styles
const notificationStyles = `
    @keyframes slideInRight {
        from {
            transform: translateX(100%);
            opacity: 0;
        }
        to {
            transform: translateX(0);
            opacity: 1;
        }
    }
    
    @keyframes slideOutRight {
        from {
            transform: translateX(0);
            opacity: 1;
        }
        to {
            transform: translateX(100%);
            opacity: 0;
        }
    }
    
    .notification-close {
        background: none;
        border: none;
        cursor: pointer;
        color: #999;
        padding: 0;
        margin-left: auto;
    }
    
    .notification-close:hover {
        color: #666;
    }
`;

// Add styles to head
const styleSheet = document.createElement('style');
styleSheet.textContent = notificationStyles;
document.head.appendChild(styleSheet);

// Real-time clock
function updateClock() {
    const now = new Date();
    const timeString = now.toLocaleTimeString('es-ES', {
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit'
    });

    const clockElement = document.getElementById('currentTime');
    if (clockElement) {
        clockElement.textContent = timeString;
    }
}

// Update clock every second
setInterval(updateClock, 1000);

// Initialize clock on load
document.addEventListener('DOMContentLoaded', updateClock);

// Handle logout confirmation
function confirmLogout() {
    if (confirm('¿Está seguro de que desea cerrar sesión?')) {
        showNotification('Cerrando sesión...', 'info');
        setTimeout(() => {
            window.location.href = '/Account/Logout';
        }, 1000);
        return true;
    }
    return false;
}

// Add logout confirmation to logout links
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.logout-link').forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            if (confirmLogout()) {
                window.location.href = this.href;
            }
        });
    });
});

// Keyboard shortcuts
document.addEventListener('keydown', function (e) {
    // Alt + D for Dashboard
    if (e.altKey && e.key === 'd') {
        e.preventDefault();
        window.location.href = '/Home/Index';
    }

    // Alt + L for Logout
    if (e.altKey && e.key === 'l') {
        e.preventDefault();
        confirmLogout();
    }

    // Escape to close dropdown
    if (e.key === 'Escape') {
        const dropdown = document.getElementById('userDropdown');
        if (dropdown && dropdown.classList.contains('show')) {
            dropdown.classList.remove('show');
        }
    }
});

// Add loading state to forms
function addLoadingToForms() {
    document.querySelectorAll('form').forEach(form => {
        form.addEventListener('submit', function () {
            const submitBtn = this.querySelector('button[type="submit"]');
            if (submitBtn) {
                submitBtn.disabled = true;
                submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Procesando...';
            }
        });
    });
}

// Initialize on DOM load
document.addEventListener('DOMContentLoaded', addLoadingToForms);

// Performance monitoring
function logPerformance() {
    if ('performance' in window) {
        const navigation = performance.getEntriesByType('navigation')[0];
        console.log(`Página cargada en: ${navigation.loadEventEnd - navigation.loadEventStart}ms`);
    }
}

window.addEventListener('load', logPerformance);

// Service worker registration (optional for PWA features)
if ('serviceWorker' in navigator) {
    window.addEventListener('load', function () {
        navigator.serviceWorker.register('/sw.js')
            .then(function (registration) {
                console.log('SW registrado exitosamente:', registration.scope);
            })
            .catch(function (error) {
                console.log('SW registro falló:', error);
            });
    });
}