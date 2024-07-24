import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  constructor(private http: HttpClient) { }

  getData(sortColumns?: { column: string, direction: string }[]): Observable<any[]> {
    let params = new HttpParams();

    if (sortColumns && sortColumns.length > 0) {
      sortColumns.forEach((column, index) => {
        params = params.set(`sortColumns[${index}].column`, column.column);
        params = params.set(`sortColumns[${index}].direction`, column.direction);
      });
    }
    return this.http.get<any[]>('https://localhost:7190/WeatherForecast/GetEmployees', { params });
  }
}
