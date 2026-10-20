
# Генерация тестовых данных для лабораторной работы № 1.
# Входные данные и ожидаемые результаты формируются на Python,
# отдельно от тестируемой программы на C#.

import csv
import math
from pathlib import Path


# Тестовые данные сгруппированы по назначению.
# Каждый элемент содержит:
# ID теста, название группы, исходный массив,
# ожидаемое количество, ожидаемую сумму.

test_cases = [
    # Группа 1. Граничные значения типа double.
    ("T01_DoubleMin", "Границы типа", [-1.7976931348623157e308], 0, 0.0),
    ("T02_Negative", "Границы типа", [-1.0], 0, 0.0),
    ("T03_Zero", "Границы типа", [0.0], 0, 0.0),
    ("T04_Positive", "Границы типа", [1.0], 0, 0.0),
    ("T05_DoubleMax", "Границы типа", [1.7976931348623157e308], 0, 0.0),

    # Группа 2. Ключевой элемент в начале, середине и конце.
    ("T06_MatchingFirst", "Положение ключевого элемента", [5, 1, 2], 1, 5.0),
    ("T07_MatchingMiddle", "Положение ключевого элемента", [1, 5, 2], 1, 5.0),
    ("T08_MatchingLast", "Положение ключевого элемента", [1, 2, 5], 1, 5.0),

    # Группа 3. Массивы без подходящих элементов.
    ("T09_Empty", "Нет ключевых элементов", [], 0, 0.0),
    ("T10_NoMatching", "Нет ключевых элементов", [1, 2, 3, 4, 6, 7, 8, 9], 0, 0.0),
    ("T11_OnlyMultiplesOf35", "Нет ключевых элементов", [35, 70, 105], 0, 0.0),
    ("T12_MultiplesOfSeven", "Нет ключевых элементов", [7, 14, 21, 28], 0, 0.0),
    ("T13_NoMatchingMixed", "Нет ключевых элементов", [14, 35, 70], 0, 0.0),
    ("T14_FractionsNoMatching", "Нет ключевых элементов", [2.5, 15.5], 0, 0.0),

    # Группа 4. Одиночные и отрицательные значения.
    ("T15_OneMatching", "Одиночные и отрицательные", [10], 1, 10.0),
    ("T16_PositiveFive", "Одиночные и отрицательные", [5], 1, 5.0),
    ("T17_NegativeFive", "Одиночные и отрицательные", [-5], 1, -5.0),
    ("T18_NotDivisibleByFive", "Одиночные и отрицательные", [14], 0, 0.0),
    ("T19_NegativeNumbers", "Одиночные и отрицательные", [-5, -10, -15, -35], 3, -30.0),
    ("T20_PositiveAndNegative", "Одиночные и отрицательные", [-10, 5, -20, 15], 4, -10.0),
    ("T21_NegativeMultiples", "Одиночные и отрицательные", [-70, -65, -60], 2, -125.0),

    # Группа 5. Смешанные данные, повторения и дробные значения.
    ("T22_MultipleMatching", "Смешанные данные", [10, 15, 20], 3, 45.0),
    ("T23_Mixed", "Смешанные данные", [10, 14, 15, 35, 20, 3, 70, -25], 4, 20.0),
    ("T24_MixedSmall", "Смешанные данные", [5, 7, 10], 2, 15.0),
    ("T25_LargerMultiples", "Смешанные данные", [25, 30, 40], 3, 95.0),
    ("T26_MixedMultiples", "Смешанные данные", [5, 35, 10, 70, 15], 3, 30.0),
    ("T27_ConsecutiveMultiples", "Смешанные данные", [5, 10, 15, 20, 25], 5, 75.0),
    ("T28_NearFifty", "Смешанные данные", [49, 50, 51], 1, 50.0),
    ("T29_NearThirtyFive", "Смешанные данные", [34, 35, 36], 0, 0.0),
    ("T30_NearSeventy", "Смешанные данные", [69, 70, 71], 0, 0.0),
    ("T31_LargerValues", "Смешанные данные", [100, 105, 110], 2, 210.0),
    ("T32_Thousand", "Смешанные данные", [999, 1000, 1001], 1, 1000.0),
    ("T33_RepeatedValues", "Смешанные данные", [10, 10, 35, 35, 15, 15], 4, 50.0),
    ("T34_MixedSequence", "Смешанные данные", [5, 7, 10, 14, 15, 21, 20, 28, 25, 35], 5, 75.0),
    ("T35_FractionalValues", "Смешанные данные", [2.5, 5.0, 10.0, 15.5], 2, 15.0),
    ("T36_ZeroAndMatching", "Смешанные данные", [0.0, 5.0, 7.0, 10.0, 35.0], 2, 15.0),
    ("T37_MixedAdditional", "Смешанные данные", [5, 35, 10], 2, 15.0),
]


def calculate_expected(numbers):
    """Вычисляет ожидаемые количество и сумму независимо на Python."""
    matching = [
        number
        for number in numbers
        if number % 5 == 0 and number % 7 != 0
    ]

    return len(matching), sum(matching)


def validate_test_cases():
    """Проверяет корректность заранее заданных ожидаемых результатов."""
    for test_id, group, numbers, expected_count, expected_sum in test_cases:
        actual_count, actual_sum = calculate_expected(numbers)

        if actual_count != expected_count or not math.isclose(
            actual_sum,
            expected_sum,
            rel_tol=1e-12,
            abs_tol=1e-12
        ):
            raise ValueError(
                f"Ошибка в тесте {test_id}: "
                f"ожидалось ({expected_count}, {expected_sum}), "
                f"получено ({actual_count}, {actual_sum})"
            )


def main():
    validate_test_cases()

    output_path = Path(__file__).resolve().parent / "test_results.csv"

    with output_path.open(
        "w",
        newline="",
        encoding="utf-8-sig"
    ) as file:
        writer = csv.writer(file, delimiter=";")

        writer.writerow([
            "ID теста",
            "Группа",
            "Исходные данные",
            "Ожидаемое количество",
            "Ожидаемая сумма"
        ])

        for test_id, group, numbers, expected_count, expected_sum in test_cases:
            writer.writerow([
                test_id,
                group,
                numbers,
                expected_count,
                expected_sum
            ])

    print("Генерация тестовых данных завершена.")
    print(f"Количество тестовых вариантов: {len(test_cases)}")
    print(f"CSV-файл сохранён: {output_path}")
    print("Все заранее заданные ожидаемые результаты проверены.")


if __name__ == "__main__":
    main()
