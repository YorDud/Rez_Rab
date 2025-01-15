﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Rez_Lab.Snatie_Photorez
{
	public partial class Snatie_Photorez_Corrector : Form
	{
		public Snatie_Photorez_Corrector()
		{
			InitializeComponent();
			//label3.Text = "Снятие фоторезиста:\r\nМодуль 1 - СНФ-725 - 8 - 12 %\r\nМодуль 2-3 - MS-358A - 8 - 12 %\r\nБак - MS-358 A - 8 - 12 %\r\nОБЪЕМ ВАННЫ: Модуль 1 - 465 л; Модуль 2-3 - 450+80 л";

		}

		private SqlDataAdapter dataAdapter;
		private DataTable dataTable;
		int[] columnsToCheck = { 8 };

		private void LoadData()
		{
			// SQL-запрос для получения данных из таблицы Users
			using (SqlConnection connection = new SqlConnection(WC.ConnectionString))
			{
				connection.Open();
				dataAdapter = new SqlDataAdapter("SELECT ID, Sn_Photorez_1_Correction, Sn_Photorez_2_Correction, Sn_Photorez_3_Correction, FIO_tech, Date_tech, FIO_Corr, Date_Corr, Сompleted,[Start_corr],[Date_start_corr],[FIO_start_corr],[Сomment] FROM Snatie_Photorez_2 ORDER BY Date_Create DESC", connection);
				SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
				dataTable = new DataTable();
				dataAdapter.Fill(dataTable);
				dataGridView1.DataSource = dataTable; // Устанавливаем источник данных

				// Изменение названий столбцов
				dataGridView1.Columns["ID"].HeaderText = "Номер";
				//dataGridView1.Columns["ID"].HeaderCell.Style.Font = new Font(dataGridView1.Font, FontStyle.Bold);
				//dataGridView1.Columns["Date_Create"].HeaderText = "Дата создания";
				//dataGridView1.Columns["FIO_Lab"].HeaderText = "ФИО Создателя";
				////
				//dataGridView1.Columns["Sn_Photorez_1_MS358A"].HeaderText = "Мод.1 MS-358A";
				dataGridView1.Columns["Sn_Photorez_1_Correction"].HeaderText = "Корректировка Мод.1";

				//dataGridView1.Columns["Sn_Photorez_2_MS358A"].HeaderText = "Мод.2 MS-358A";
				dataGridView1.Columns["Sn_Photorez_2_Correction"].HeaderText = "Корректировка Мод.2";

				//dataGridView1.Columns["Sn_Photorez_3_MS358A"].HeaderText = "Бак MS-358A";
				dataGridView1.Columns["Sn_Photorez_3_Correction"].HeaderText = "Корректировка Бак";
				//
				dataGridView1.Columns["FIO_tech"].HeaderText = "ФИО Технолога";
				dataGridView1.Columns["Date_tech"].HeaderText = "Дата создания корректировки";
				//dataGridView1.Columns["FIO_Lab_Update"].HeaderText = "ФИО Лаборанта (редакт)";
				//dataGridView1.Columns["Date_Lab_Update"].HeaderText = "Дата (редакт)";
				dataGridView1.Columns["FIO_Corr"].HeaderText = "ФИО Корректировщика";
				dataGridView1.Columns["Date_Corr"].HeaderText = "Время выполнения корректировки";
				dataGridView1.Columns["Сompleted"].HeaderText = "Выполнение";
dataGridView1.Columns["Start_corr"].HeaderText = "Принятие в работу";
dataGridView1.Columns["Date_start_corr"].HeaderText = "Время принятия корректировки в выполнение";
dataGridView1.Columns["FIO_start_corr"].HeaderText = "ФИО Корректировщика";
dataGridView1.Columns["Сomment"].HeaderText = "Комментарий";

				// Продолжайте добавлять другие столбцы по мере необходимости
				
foreach (DataGridViewRow row in dataGridView1.Rows)
{
				// Проверяем, что значение в ячейке не null
				if (row.Cells["Сomment"].Value != null)
				{
					string comment = row.Cells["Сomment"].Value.ToString();
					if (comment.Contains("НР"))
					{
						// Устанавливаем синий цвет фона и белый цвет текста
						row.DefaultCellStyle.BackColor = Color.LightSkyBlue;
						//row.DefaultCellStyle.ForeColor = Color.White;
					}
				}
}
HighlightEmptyCells(dataGridView1, columnsToCheck);
			}

		}


		private void pictureBox1_Click(object sender, EventArgs e)
		{
			this.Hide();
		}

		private void Snatie_Photorez_Corrector_Load(object sender, EventArgs e)
		{
			LoadData();
			label8.Text = WC.fio.ToString();
			//this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
			HighlightEmptyCells(dataGridView1, columnsToCheck);
			NotClickOnTable();
		}
		private void NotClickOnTable()
		{
			foreach (DataGridViewColumn column in dataGridView1.Columns)
			{
				if (column.Name != "R") // предполагается, что "ID" - это столбец, который можно редактировать
				{
					column.ReadOnly = true;
				}
			}
		}

		private void LoadDataDateTimePicker(DateTime date)
		{
			using (SqlConnection connection = new SqlConnection(WC.ConnectionString))
			{
				connection.Open();

				string query = "SELECT ID, Sn_Photorez_1_Correction, Sn_Photorez_2_Correction, Sn_Photorez_3_Correction, FIO_tech, Date_tech, FIO_Corr, Date_Corr, Сompleted,[Start_corr],[Date_start_corr],[FIO_start_corr],[Сomment] FROM Snatie_Photorez_2 WHERE CAST(Date_Create AS DATE) = @SelectedDate ORDER BY Date_Create DESC";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@SelectedDate", date);

					SqlDataAdapter adapter = new SqlDataAdapter(command);
					DataTable dataTable = new DataTable();
					adapter.Fill(dataTable);

					dataGridView1.DataSource = dataTable; // Отображаем данные в DataGridView
				}
			}
		}

		private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
		{
			DateTime selectedDate = dateTimePicker1.Value.Date; // Получаем выбранную дату
			LoadDataDateTimePicker(selectedDate);
		}

		private void Refresh_btn_Click(object sender, EventArgs e)
		{
			LoadData();
			HighlightEmptyCells(dataGridView1, columnsToCheck);
		}

		private void HighlightEmptyCells(DataGridView dataGridView, int[] columns)
		{
			foreach (DataGridViewRow row in dataGridView.Rows)
			{
				// Проверяем, что значение в 6-м столбике (индекс 5) не пустое
				if (!string.IsNullOrEmpty(row.Cells[1].Value?.ToString()) || !string.IsNullOrEmpty(row.Cells[2].Value?.ToString()) || !string.IsNullOrEmpty(row.Cells[3].Value?.ToString()) || !string.IsNullOrEmpty(row.Cells[12].Value?.ToString()))
				{
					foreach (int columnIndex in columns)
					{
						if (string.IsNullOrEmpty(row.Cells[columnIndex].Value?.ToString()))
						{
							row.Cells[columnIndex].Style.BackColor = Color.Red;
							row.DefaultCellStyle.BackColor = Color.LightSalmon;
						}
						if (string.IsNullOrEmpty(row.Cells[8].Value?.ToString()) && string.IsNullOrEmpty(row.Cells[9].Value?.ToString()))
						{
							row.Cells[9].Style.BackColor = Color.Yellow;
							//row.DefaultCellStyle.BackColor = Color.LightSalmon;
						}
						if (!string.IsNullOrEmpty(row.Cells[9].Value?.ToString()) && string.IsNullOrEmpty(row.Cells[8].Value?.ToString()))
						{
							//row.Cells[8].Style.BackColor = Color.Yellow;
							row.DefaultCellStyle.BackColor = Color.FromArgb(217, 253, 38);
						}
					}
				}
			}
		}
		



		int clickCount = 0;
		private void button3_Click(object sender, EventArgs e)
		{
			clickCount++;

			switch (clickCount)
			{
				case 1:
					groupBox1.Visible = true;
					break;
				case 2:
					groupBox1.Visible = false;
					clickCount = 0;
					LoadData();
					break;

			}

			//groupBox3.Visible = true;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			DateTime startDate = dateTimePicker2.Value.Date;
			DateTime endDate = dateTimePicker3.Value.Date;

			// Проверка, что начальная дата не позже конечной
			if (startDate > endDate)
			{
				MessageBox.Show("Начальная дата не может быть позже конечной даты.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// Дальнейшие действия с диапазоном дат
			//MessageBox.Show($"Выбранный диапазон дат:\nС: {startDate.ToShortDateString()}\nПо: {endDate.ToShortDateString()}");

			// Загрузка данных на основании выбранного диапазона
			LoadDataByDateRange(startDate, endDate);
			HighlightEmptyCells(dataGridView1, columnsToCheck);
		}

		private void LoadDataByDateRange(DateTime startDate, DateTime endDate)
		{
			using (SqlConnection connection = new SqlConnection(WC.ConnectionString))
			{
				connection.Open();
				string query = "SELECT ID, Sn_Photorez_1_Correction, Sn_Photorez_2_Correction, Sn_Photorez_3_Correction, FIO_tech, Date_tech, FIO_Corr, Date_Corr, Сompleted,[Start_corr],[Date_start_corr],[FIO_start_corr],[Сomment] FROM Snatie_Photorez_2 WHERE CAST(Date_Create AS DATE) BETWEEN @StartDate AND @EndDate ORDER BY Date_Create DESC";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@StartDate", startDate);
					command.Parameters.AddWithValue("@EndDate", endDate);

					SqlDataAdapter adapter = new SqlDataAdapter(command);
					DataTable dataTable = new DataTable();
					adapter.Fill(dataTable);
					dataGridView1.DataSource = dataTable; // Отображаем данные в DataGridView
				}
			}
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			// Проверяем, что клик был совершен по существующей строке и по 15-му столбцу (индекс 14)
			if (e.RowIndex >= 0 && columnsToCheck.Contains(e.ColumnIndex))
			{
				// Получаем текущую строку
				DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

				// Получаем значения необходимых столбцов
				var completedValue = selectedRow.Cells["Сompleted"].Value;
				var Sn_Photorez_1_Correction_Value = selectedRow.Cells["Sn_Photorez_1_Correction"].Value;
				var Sn_Photorez_2_Correction_Value = selectedRow.Cells["Sn_Photorez_2_Correction"].Value;
				var Sn_Photorez_3_Correction_Value = selectedRow.Cells["Sn_Photorez_3_Correction"].Value;
				var СommentValue = selectedRow.Cells["Сomment"].Value;

				var startValue = selectedRow.Cells["Start_corr"].Value;
				bool isStart = startValue != null &&
										  startValue != DBNull.Value &&
										  !(startValue is string strstart && string.IsNullOrWhiteSpace(strstart));

				if (!isStart)
				{
					MessageBox.Show("Нельзя выполнить корректировку без начала ее выполнения! Редактирование недоступно.",
									"Информация",
									MessageBoxButtons.OK,
									MessageBoxIcon.Information);
					return;
				}

				// Проверяем, что "Completed" пустой (null, DBNull или пустая строка)
				bool isCompletedEmpty = completedValue == null ||
										 completedValue == DBNull.Value ||
										 (completedValue is string strCompleted && string.IsNullOrWhiteSpace(strCompleted));

				// Проверяем, что "Date_tech" не пустой
				bool isSn_Photorez_1_CorrectionhNotEmpty = Sn_Photorez_1_Correction_Value != null &&
										  Sn_Photorez_1_Correction_Value != DBNull.Value &&
										  !(Sn_Photorez_1_Correction_Value is string strCorr1 && string.IsNullOrWhiteSpace(strCorr1));

				// Проверяем, что "FIO_tech" не пустой
				bool isSn_Photorez_2_CorrectionNotEmpty = Sn_Photorez_2_Correction_Value != null &&
										 Sn_Photorez_2_Correction_Value != DBNull.Value &&
										 !(Sn_Photorez_2_Correction_Value is string strCorr2 && string.IsNullOrWhiteSpace(strCorr2));

				bool isSn_Photorez_3_CorrectionNotEmpty = Sn_Photorez_3_Correction_Value != null &&
										 Sn_Photorez_3_Correction_Value != DBNull.Value &&
										 !(Sn_Photorez_3_Correction_Value is string strCorr3 && string.IsNullOrWhiteSpace(strCorr3));

				bool isСommentNotEmpty = СommentValue != null &&
										 СommentValue != DBNull.Value &&
										 !(СommentValue is string strComm && string.IsNullOrWhiteSpace(strComm));

				// Если "Completed" не пустой, показать сообщение и выйти из метода
				if (!isCompletedEmpty)
				{
					MessageBox.Show("Эта корректировка уже выполнена! Редактирование недоступно.",
									"Информация",
									MessageBoxButtons.OK,
									MessageBoxIcon.Information);
					return;
				}

				// Если "Date_tech" пустой, показать сообщение и выйти из метода
				if (!isSn_Photorez_1_CorrectionhNotEmpty && !isSn_Photorez_2_CorrectionNotEmpty && !isSn_Photorez_3_CorrectionNotEmpty && !isСommentNotEmpty)
				{
					MessageBox.Show("Выполнение без корректироки и комментария невозможно! Редактирование недоступно.",
									"Предупреждение",
									MessageBoxButtons.OK,
									MessageBoxIcon.Warning);
					return;
				}

				// Получаем значение ID из первого столбца (индекс 0)
				var idValue = selectedRow.Cells[0].Value;

				// Проверяем, что ID не пустой
				if (idValue != null)
				{
					if (int.TryParse(idValue.ToString(), out int idClick))
					{
						// Подготовка SQL-запроса для обновления
						string completed = "Выполнено!";
						string connectionString = WC.ConnectionString;
						string query = @"UPDATE Snatie_Photorez_2 SET 
                                 [Date_Corr] = @Date_Corr,
                                 [Сompleted] = @Сompleted,
                                 [FIO_Corr] = @FIO_Corr
                                 WHERE ID = @Id";

						try
						{
							using (SqlConnection connection = new SqlConnection(connectionString))
							{
								using (SqlCommand command = new SqlCommand(query, connection))
								{
									// Добавляем параметры с соответствующими значениями
									command.Parameters.AddWithValue("@Date_Corr", DateTime.Now);
									command.Parameters.AddWithValue("@Сompleted", completed);
									command.Parameters.AddWithValue("@FIO_Corr", WC.fio);
									command.Parameters.AddWithValue("@Id", idClick);

									// Отладочное сообщение (можно удалить в продакшен версии)
									//Console.WriteLine($"Executing UPDATE with ID: {idClick}, Date_Corr: {DateTime.Now}, Completed: {completed}, FIO_Corr: {WC.fio}");

									connection.Open();
									int rowsAffected = command.ExecuteNonQuery();

									if (rowsAffected > 0)
									{
										MessageBox.Show("Корректировка выполнена!",
														"Успех",
														MessageBoxButtons.OK,
														MessageBoxIcon.Information);
										LoadData(); // Обновляем данные в DataGridView
									}
									else
									{
										MessageBox.Show("Не удалось обновить данные.",
														"Ошибка",
														MessageBoxButtons.OK,
														MessageBoxIcon.Error);
									}
								}
							}
						}
						catch (Exception ex)
						{
							// Обработка возможных исключений
							MessageBox.Show("Произошла ошибка при обновлении данных: " + ex.Message,
											"Ошибка",
											MessageBoxButtons.OK,
											MessageBoxIcon.Error);
						}
					}
					else
					{
						MessageBox.Show("Не удалось преобразовать значение ID.",
										"Ошибка",
										MessageBoxButtons.OK,
										MessageBoxIcon.Error);
					}
				}
				else
				{
					MessageBox.Show("Значение ID отсутствует.",
									"Ошибка",
									MessageBoxButtons.OK,
									MessageBoxIcon.Error);
				}
			}
			else if (e.RowIndex >= 0 && e.ColumnIndex == 9)
			{
				// Получаем текущую строку
				DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

				// Получаем значения необходимых столбцов
				var completedValue = selectedRow.Cells["Start_corr"].Value;
				var Sn_Photorez_1_Correction_Value = selectedRow.Cells["Sn_Photorez_1_Correction"].Value;
				var Sn_Photorez_2_Correction_Value = selectedRow.Cells["Sn_Photorez_2_Correction"].Value;
				var Sn_Photorez_3_Correction_Value = selectedRow.Cells["Sn_Photorez_3_Correction"].Value;
				var СommentValue = selectedRow.Cells["Сomment"].Value;

				// Проверяем, что "Completed" пустой (null, DBNull или пустая строка)
				bool isCompletedEmpty = completedValue == null ||
										 completedValue == DBNull.Value ||
										 (completedValue is string strCompleted && string.IsNullOrWhiteSpace(strCompleted));

				// Проверяем, что "Date_tech" не пустой
				bool isSn_Photorez_1_CorrectionhNotEmpty = Sn_Photorez_1_Correction_Value != null &&
										  Sn_Photorez_1_Correction_Value != DBNull.Value &&
										  !(Sn_Photorez_1_Correction_Value is string strCorr1 && string.IsNullOrWhiteSpace(strCorr1));

				// Проверяем, что "FIO_tech" не пустой
				bool isSn_Photorez_2_CorrectionNotEmpty = Sn_Photorez_2_Correction_Value != null &&
										 Sn_Photorez_2_Correction_Value != DBNull.Value &&
										 !(Sn_Photorez_2_Correction_Value is string strCorr2 && string.IsNullOrWhiteSpace(strCorr2));

				bool isSn_Photorez_3_CorrectionNotEmpty = Sn_Photorez_3_Correction_Value != null &&
										 Sn_Photorez_3_Correction_Value != DBNull.Value &&
										 !(Sn_Photorez_3_Correction_Value is string strCorr3 && string.IsNullOrWhiteSpace(strCorr3));

				bool isСommentNotEmpty = СommentValue != null &&
										 СommentValue != DBNull.Value &&
										 !(СommentValue is string strComm && string.IsNullOrWhiteSpace(strComm));

				// Если "Completed" не пустой, показать сообщение и выйти из метода
				if (!isCompletedEmpty)
				{
					MessageBox.Show("Эта корректировка уже выполняется! Редактирование недоступно.",
									"Информация",
									MessageBoxButtons.OK,
									MessageBoxIcon.Information);
					return;
				}

				// Если "Date_tech" пустой, показать сообщение и выйти из метода
				if (!isSn_Photorez_1_CorrectionhNotEmpty && !isSn_Photorez_2_CorrectionNotEmpty && !isSn_Photorez_3_CorrectionNotEmpty && !isСommentNotEmpty)
				{
					MessageBox.Show("Выполнение без корректироки и комментария невозможно! Редактирование недоступно.",
									"Предупреждение",
									MessageBoxButtons.OK,
									MessageBoxIcon.Warning);
					return;
				}

				// Получаем значение ID из первого столбца (индекс 0)
				var idValue = selectedRow.Cells[0].Value;

				// Проверяем, что ID не пустой
				if (idValue != null)
				{
					if (int.TryParse(idValue.ToString(), out int idClick))
					{
						// Подготовка SQL-запроса для обновления
						string completed = "В работе";
						string connectionString = WC.ConnectionString;
						string query = @"UPDATE Snatie_Photorez_2 SET 
                                 [Date_start_corr] = @Date_start_corr,
                                 [Start_corr] = @Start_corr,
                                 [FIO_start_corr] = @FIO_start_corr
                                 WHERE ID = @Id";
						
						try
						{
							using (SqlConnection connection = new SqlConnection(connectionString))
							{
								using (SqlCommand command = new SqlCommand(query, connection))
								{
									// Добавляем параметры с соответствующими значениями
									command.Parameters.AddWithValue("@Date_start_corr", DateTime.Now);
									command.Parameters.AddWithValue("@Start_corr", completed);
									command.Parameters.AddWithValue("@FIO_start_corr", WC.fio);
									command.Parameters.AddWithValue("@Id", idClick);

									// Отладочное сообщение (можно удалить в продакшен версии)
									//Console.WriteLine($"Executing UPDATE with ID: {idClick}, Date_Corr: {DateTime.Now}, Completed: {completed}, FIO_Corr: {WC.fio}");

									connection.Open();
									int rowsAffected = command.ExecuteNonQuery();

									if (rowsAffected > 0)
									{
										MessageBox.Show("Корректировка принята в работу!",
														"Успех",
														MessageBoxButtons.OK,
														MessageBoxIcon.Information);
										LoadData(); // Обновляем данные в DataGridView
									}
									else
									{
										MessageBox.Show("Не удалось обновить данные.",
														"Ошибка",
														MessageBoxButtons.OK,
														MessageBoxIcon.Error);
									}
								}
							}
						}
						catch (Exception ex)
						{
							// Обработка возможных исключений
							MessageBox.Show("Произошла ошибка при обновлении данных: " + ex.Message,
											"Ошибка",
											MessageBoxButtons.OK,
											MessageBoxIcon.Error);
						}
					}
					else
					{
						MessageBox.Show("Не удалось преобразовать значение ID.",
										"Ошибка",
										MessageBoxButtons.OK,
										MessageBoxIcon.Error);
					}
				}
				else
				{
					MessageBox.Show("Значение ID отсутствует.",
									"Ошибка",
									MessageBoxButtons.OK,
									MessageBoxIcon.Error);
				}
			}
		}

		private void button4_Click(object sender, EventArgs e)
		{
			// Получить данные из базы данных
			dataSet = GetAllDataFromDatabase();
			PrintDocument printDoc = new PrintDocument();

			printDoc.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
			PrintDialog printDialog = new PrintDialog
			{
				Document = printDoc
			};

			if (printDialog.ShowDialog() == DialogResult.OK)
			{
				printDoc.Print();
			}
		}



		private DataSet dataSet;
		private readonly string[] tableHeaders = new string[]
		{
		"!!! Снятие фоторезиста !!!"
		};
		private int currentTableIndex = 0;
		private int currentRowIndex = 0;
		private readonly float[] columnWidths = { 170, 170, 170, 180 };
		private System.Drawing.Font titleFont = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
		private System.Drawing.Font headerFont = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
		private System.Drawing.Font columnFont = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
		private System.Drawing.Font dataFont = new System.Drawing.Font("Arial", 12, FontStyle.Bold);

		public void Print()
		{
			dataSet = GetAllDataFromDatabase();
			PrintDocument printDoc = new PrintDocument();

			printDoc.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
			PrintDialog printDialog = new PrintDialog
			{
				Document = printDoc
			};

			if (printDialog.ShowDialog() == DialogResult.OK)
			{
				printDoc.Print();
			}
		}

		private DataSet GetAllDataFromDatabase()
		{
			DataSet ds = new DataSet();
			string query = @"
            SELECT 
                [Sn_Photorez_1_Correction],
                [Sn_Photorez_2_Correction],
                [Sn_Photorez_3_Correction],
                [Сomment]
            FROM 
                Snatie_Photorez_2
            WHERE 
                ([Сompleted] IS NULL OR [Сompleted] = '') 
                AND (
                    ([Sn_Photorez_1_Correction] IS NOT NULL AND [Sn_Photorez_1_Correction] <> '') 
                    OR ([Сomment] IS NOT NULL AND [Сomment] <> '') OR ([Sn_Photorez_2_Correction] IS NOT NULL AND [Sn_Photorez_2_Correction] <> '')
                    OR ([Sn_Photorez_3_Correction] IS NOT NULL AND [Sn_Photorez_3_Correction] <> '')
                );
        ";

			using (SqlConnection connection = new SqlConnection(WC.ConnectionString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				SqlDataAdapter adapter = new SqlDataAdapter(command);
				connection.Open();
				adapter.Fill(ds);
			}

			return ds;
		}

		private Font dateFont = new Font("Arial", 14, FontStyle.Bold);
		private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
		{
			float yPos = e.MarginBounds.Top;
			float leftMargin = e.MarginBounds.Left;
			float lineHeight = e.Graphics.MeasureString("Sample", dataFont).Height;


			string currentDateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

			// Измеряем размер строки даты и времени
			SizeF dateSize = e.Graphics.MeasureString(currentDateTime, dateFont);

			// Вычисляем координату X для центра (центр области печати минус половина ширины строки)
			float dateX = leftMargin + (e.MarginBounds.Width - dateSize.Width) / 2;

			// Рисуем строку даты и времени по рассчитанной позиции
			e.Graphics.DrawString(currentDateTime, dateFont, Brushes.Black, dateX, yPos);

			// Обновляем yPos с учетом высоты строки и отступа
			yPos += dateFont.GetHeight(e.Graphics) + 10;

			// 1. Печать заголовка "MENYAY" по центру верхней части страницы
			string title = "Снятие фоторезиста";
			SizeF titleSize = e.Graphics.MeasureString(title, titleFont);
			float titleX = leftMargin + (e.MarginBounds.Width - titleSize.Width) / 2;
			e.Graphics.DrawString(title, titleFont, Brushes.Black, titleX, yPos);
			yPos += titleSize.Height + 20; // Отступ после заголовка


			float rowSpacing = 8.0f;
			// Объект StringFormat с настройками переноса текста
			StringFormat stringFormat = new StringFormat();
			stringFormat.Trimming = StringTrimming.Word; // Обрезка по словам
			stringFormat.FormatFlags = StringFormatFlags.LineLimit; // Ограничение числа строк

			while (currentTableIndex < dataSet.Tables.Count)
			{
				System.Data.DataTable table = dataSet.Tables[currentTableIndex];
				string header = tableHeaders.Length > currentTableIndex ? tableHeaders[currentTableIndex] : $"Таблица {currentTableIndex + 1}";

				// Печать заголовка таблицы
				e.Graphics.DrawString(header, headerFont, Brushes.Black, leftMargin, yPos, stringFormat);
				yPos += lineHeight + 5;

				// Пользовательские названия столбцов
				string[] customColumnNames = { "Корр. Мод.1", "Корр. Мод.2-3", "Корр. Бак", "Комментарии" };
				float currentLeft = leftMargin;

				// Печать заголовков столбцов
				for (int i = 0; i < customColumnNames.Length; i++)
				{
					RectangleF headerRect = new RectangleF(currentLeft, yPos, columnWidths[i], lineHeight);
					e.Graphics.DrawString(customColumnNames[i], columnFont, Brushes.Black, headerRect, stringFormat);
					currentLeft += columnWidths[i];
				}
				yPos += lineHeight;

				// Печать строк данных
				while (currentRowIndex < table.Rows.Count)
				{
					DataRow row = table.Rows[currentRowIndex];
					currentLeft = leftMargin;

					// Определяем максимальную высоту для текущей строки
					float maxHeight = 0;
					SizeF[] sizes = new SizeF[customColumnNames.Length];

					for (int i = 0; i < customColumnNames.Length; i++)
					{
						string text = row[i]?.ToString() ?? string.Empty;
						// Вычисляем размер текста с учетом переноса
						sizes[i] = e.Graphics.MeasureString(text, dataFont, (int)columnWidths[i], stringFormat);
						if (sizes[i].Height > maxHeight)
						{
							maxHeight = sizes[i].Height;
						}
					}

					// Проверка границ страницы перед печатью строки
					if (yPos + maxHeight > e.MarginBounds.Bottom)
					{
						e.HasMorePages = true;
						return;
					}

					// Печать каждой ячейки в строке с переносом текста
					for (int i = 0; i < customColumnNames.Length; i++)
					{
						string text = row[i]?.ToString() ?? string.Empty;
						RectangleF cellRect = new RectangleF(currentLeft, yPos, columnWidths[i], maxHeight);
						e.Graphics.DrawString(text, dataFont, Brushes.Black, cellRect, stringFormat);
						currentLeft += columnWidths[i];
					}

					// Увеличиваем yPos на высоту строки плюс отступ
					yPos += maxHeight + rowSpacing;
					currentRowIndex++;
				}

				// Завершение текущей таблицы
				currentRowIndex = 0;
				currentTableIndex++;
				yPos += lineHeight; // Пустая строка между таблицами

				// Проверка границ страницы после добавления пустой строки
				if (yPos + lineHeight > e.MarginBounds.Bottom)
				{
					e.HasMorePages = true;
					return;
				}
			}

			// Все таблицы напечатаны
			e.HasMorePages = false;

			// Сброс индексов для следующей печати
			currentTableIndex = 0;
			currentRowIndex = 0;
		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			pictureBox2.Visible = !pictureBox2.Visible;
		}
	}

			
}
