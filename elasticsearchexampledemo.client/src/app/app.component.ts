import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { EmployeeService } from './EmployeeService'

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  // public forecasts: WeatherForecast[] = [];
  employees: any[] = [];
  sortColumns: { column: string, direction: string }[] = [];
  // constructor(private http: HttpClient) {}
  constructor(private employeeService: EmployeeService) { }

  ngOnInit() {
    this.loadData();
  }
  loadData() {
    this.employeeService.getData(this.sortColumns).subscribe(data => {
      console.log(data);
      this.employees = data;
    });
  }
  toggleSort(column: string) {
    const sortColumn = this.sortColumns.find(c => c.column === column);

    if (sortColumn) {
      sortColumn.direction = sortColumn.direction === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumns.push({ column, direction: 'asc' });
    }

    this.loadData();
  }
  // getForecasts() {
  //   this.http.get<WeatherForecast[]>('/weatherforecast').subscribe(
  //     (result) => {
  //       this.forecasts = result;
  //     },
  //     (error) => {
  //       console.error(error);
  //     }
  //   );
  // }

  title = 'elasticsearchexampledemo.client';
}
