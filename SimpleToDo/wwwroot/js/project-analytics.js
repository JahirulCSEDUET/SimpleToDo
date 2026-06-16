document.addEventListener('DOMContentLoaded', function () {

    const doughnutCanvas = document.getElementById('statusDoughnutChart');
    if (doughnutCanvas) {
        const ctxDoughnut = doughnutCanvas.getContext('2d');

        const pendingCount = parseInt(doughnutCanvas.getAttribute('data-pending')) || 0;
        const progressCount = parseInt(doughnutCanvas.getAttribute('data-progress')) || 0;
        const completedCount = parseInt(doughnutCanvas.getAttribute('data-completed')) || 0;
        const unassignedCount = parseInt(doughnutCanvas.getAttribute('data-unassigned')) || 0;

        new Chart(ctxDoughnut, {
            type: 'doughnut',
            data: {
                labels: ['Todo', 'In Progress', 'Completed', 'Unassigned'],
                datasets: [{
                    data: [pendingCount, progressCount, completedCount, unassignedCount],
                    backgroundColor: [
                        'rgba(220, 53, 69, 0.85)',   
                        'rgba(13, 110, 253, 0.85)',  
                        'rgba(25, 135, 84, 0.85)',   
                        'rgba(108, 117, 125, 0.4)'   
                    ],
                    borderWidth: 2,
                    borderColor: '#ffffff'
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: { boxWidth: 12, padding: 15, font: { size: 11, weight: '500' } }
                    }
                },
                cutout: '70%'
            }
        });
    }

    // 📈 2. BAR CHART SETUP: Team Workload Balance
    const barCanvas = document.getElementById('workloadBarChart');
    if (barCanvas) {
        const ctxBar = barCanvas.getContext('2d');

        // Safely parse arrays passed from server JSON attributes
        const teamLabels = JSON.parse(barCanvas.getAttribute('data-labels') || '[]');
        const teamTodo = JSON.parse(barCanvas.getAttribute('data-todo') || '[]');
        const teamProgress = JSON.parse(barCanvas.getAttribute('data-progress') || '[]');
        const teamDone = JSON.parse(barCanvas.getAttribute('data-done') || '[]');

        new Chart(ctxBar, {
            type: 'bar',
            data: {
                labels: teamLabels,
                datasets: [
                    {
                        label: 'Todo',
                        data: teamTodo,
                        backgroundColor: 'rgba(220, 53, 69, 0.8)'
                    },
                    {
                        label: 'In Progress',
                        data: teamProgress,
                        backgroundColor: 'rgba(13, 110, 253, 0.8)'
                    },
                    {
                        label: 'Completed',
                        data: teamDone,
                        backgroundColor: 'rgba(25, 135, 84, 0.8)'
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    x: {
                        stacked: true,
                        grid: { display: false }
                    },
                    y: {
                        stacked: true,
                        ticks: { precision: 0 },
                        beginAtZero: true
                    }
                },
                plugins: {
                    legend: {
                        position: 'top',
                        labels: { boxWidth: 12, font: { size: 11, weight: '500' } }
                    }
                }
            }
        });
    }
});